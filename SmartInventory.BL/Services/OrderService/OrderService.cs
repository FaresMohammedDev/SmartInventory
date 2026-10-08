using SmartInventory.BL.Common;
using SmartInventory.BL.DTOs.Order;
using SmartInventory.DAL.Models;
using SmartInventory.DAL.Repositories.Interfaces;
using SmartInventory.BL.DTOs.Item;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IProductRepo _productRepo;
        private readonly IItemRepo _itemRepo;
        public OrderService(IOrderRepo orderRepo, IProductRepo productRepo, IItemRepo itemRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _itemRepo = itemRepo;
        }

        public async Task<ServiceResponse<bool>> CancelAsync(int id, int userId)
        {
            var order = await _orderRepo.GetOrderWithItemsAsync(id);
            if (order == null)
                return ServiceResponse<bool>.Fail("Order not found");

            if (order.userId != userId)
                return ServiceResponse<bool>.Fail("This Order not for you");

            var items = await _itemRepo.FindAsync(x => x.orderId == id);
            foreach (var item in items)
            {
                var product = await _productRepo.GetByIdAsync(item.productId);
                if (product == null)
                    return ServiceResponse<bool>.Fail($"Product with Id. {item.productId} not found");

                product.Quantity += item.Quantity;

                await _productRepo.UpdateAsync(product);

                await _itemRepo.DeleteAsync(item);
            }

            await _orderRepo.DeleteAsync(order);
            await _orderRepo.SaveChangesAsync();
            await _productRepo.SaveChangesAsync();
            await _itemRepo.SaveChangesAsync();

            return ServiceResponse<bool>.Success(true);
        }

        public async Task<ServiceResponse<int>> CreateAsync(CreateOrderDto orderDto, int userId)
        {
            decimal totalPrice = 0;

            var newOrder = new Order
            {
                userId = userId,
                DateTime = orderDto.DateTime,
                Items = new List<Item>()
            };

            foreach(var itemDto in orderDto.Items)
            {
                var item = itemDto;
                var product = await _productRepo.GetByIdAsync(item.productId);
                if (product == null)
                    return ServiceResponse<int>.Fail($"Product {item.productId} not found");

                if (product.Quantity < item.Quantity)
                    return ServiceResponse<int>.Fail($"Quantity of {product.Id}. {product.Title} is {product.Quantity}");

                product.Quantity -= item.Quantity;
                await _productRepo.UpdateAsync(product);

                totalPrice += (product.Price * item.Quantity);

                newOrder.Items.Add(new Item
                {
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    orderId = newOrder.Id,
                    productId = product.Id
                });
            }

            newOrder.TotalPrice = totalPrice;

            await _orderRepo.CreateAsync(newOrder);
            await _orderRepo.SaveChangesAsync();

            return ServiceResponse<int>.Success(newOrder.Id);
        }

        public async Task<ServiceResponse<List<GetOrderDto>>> GetAllAsync()
        {
            var orders = await _orderRepo.GetAllWithItemsAsync();
            if (orders == null || !orders.Any())
                return ServiceResponse<List<GetOrderDto>>.Fail("No Orders yet");

            var ordersDto = orders.Select(x => new GetOrderDto
            {
                Id = x.Id,
                DateTime = x.DateTime,
                TotalPrice = x.TotalPrice,
                UserName = x.User?.FullName ?? "Unkown",
                Items = x.Items.Select(y => new GetItemDto
                {
                    Id = y.Id,
                    orderId = y.orderId,
                    productName = y.Product?.Title ?? "Product",
                    Quantity = y.Quantity,
                    UnitPrice= y.UnitPrice,
                }).ToList()
            }).ToList();

            return ServiceResponse<List<GetOrderDto>>.Success(ordersDto);
        }

        public async Task<ServiceResponse<GetOrderDto>> GetByIdAsync(int id, int userId)
        {
            var order = await _orderRepo.GetOrderWithItemsAsync(id);
            if (order == null)
                return ServiceResponse<GetOrderDto>.Fail("Order not found");

            if (order.userId != userId)
                return ServiceResponse<GetOrderDto>.Fail("This order not for you");

            var orderDto = new GetOrderDto
            {
                Id = id,
                UserName = order.User?.FullName ?? "Unkown",
                TotalPrice = order.TotalPrice,
                DateTime = order.DateTime,
                Items = order.Items.Select(item => new GetItemDto
                {
                    Id = item.Id,
                    productName = item.Product?.Title ?? "Product",
                    orderId = item.orderId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            return ServiceResponse<GetOrderDto>.Success(orderDto);
        }

        public async Task<ServiceResponse<List<GetOrderDto>>> GetByUserIdAsync(int userId)
        {
            var orders = await _orderRepo.FindWithAsync(userId);
            if (orders == null || !orders.Any())
                return ServiceResponse<List<GetOrderDto>>.Fail("This User dont have orders");

            var ordersDto = orders.Select(order => new GetOrderDto
            {
                Id = order.Id,
                UserName = order.User?.UserName ?? "Unkown",
                DateTime = order.DateTime,
                TotalPrice = order.TotalPrice,
                Items = order.Items.Select(item => new GetItemDto
                {
                    Id = item.Id,
                    orderId = order.Id,
                    productName = item.Product?.Title ?? "Product",
                    Quantity = item.Quantity,
                    UnitPrice= item.UnitPrice
                }).ToList()
            }).ToList();

            return ServiceResponse<List<GetOrderDto>>.Success(ordersDto);
        }

        public async Task<ServiceResponse<PaginatedResult<GetOrderDto>>> GetPaginatedAsync(int userId, int pageIndex, int pageSize)
        {
            var (orders, totalCount) = await _orderRepo.GetPaginatedAsync(userId, pageIndex, pageSize);

            var ordersDto = orders.Select(x => new GetOrderDto
            {
                Id = x.Id,
                UserName = x.User?.UserName ?? "Unkown",
                TotalPrice = x.TotalPrice,
                DateTime = x.DateTime,
                Items = x.Items.Select(item => new GetItemDto
                {
                    Id = item.Id,
                    orderId = item.orderId,
                    productName = item.Product?.Title ?? "Product",
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            }).ToList();

            var result = new PaginatedResult<GetOrderDto>
            {
                Data = ordersDto,
                PageNumber = pageIndex,
                PageSize = pageSize,
                totalRecords = totalCount
            };

            return ServiceResponse<PaginatedResult<GetOrderDto>>.Success(result);
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(UpdateOrderDto orderDto, int id, int userId)
        {
            var order = await _orderRepo.GetOrderWithItemsAsync(id);
            if (order == null)
                return ServiceResponse<bool>.Fail("Order not found");

            if (order.userId != userId)
                return ServiceResponse<bool>.Fail("This Order not for you");

            // Delete Old Items
            foreach (var i in order.Items)
            {
                var item = i;
                var product = await _productRepo.GetByIdAsync(item.productId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                    await _productRepo.UpdateAsync(product);
                }

                await _itemRepo.DeleteAsync(item);
            }

            // Add new Items
            decimal newTotalPrice = 0;
            var newItems = new List<Item>();
            foreach (var i in orderDto.items)
            {
                var item = i;
                var product = await _productRepo.GetByIdAsync(item.productId);
                if (product == null)
                    return ServiceResponse<bool>.Fail($"Product with ID {item.productId} not found.");

                if (product.Quantity < item.Quantity)
                    return ServiceResponse<bool>.Fail($"Not enough stock for {product.Title}. Available: {product.Quantity}");

                product.Quantity -= item.Quantity;
                await _productRepo.UpdateAsync(product);

                newTotalPrice += (item.Quantity * product.Price);

                order.Items.Add(new Item
                {
                    Quantity = item.Quantity,
                    orderId = order.Id,
                    productId = product.Id,
                    UnitPrice = product.Price
                });
            }

            order.TotalPrice = newTotalPrice;
            order.DateTime = orderDto.DateTime;

            await _orderRepo.UpdateAsync(order);
            await _orderRepo.SaveChangesAsync();
            await _productRepo.SaveChangesAsync();
            await _itemRepo.SaveChangesAsync();

            return ServiceResponse<bool>.Success(true);
        }
    }
}
