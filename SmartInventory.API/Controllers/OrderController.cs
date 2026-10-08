using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.BL.DTOs.Order;
using SmartInventory.BL.Services.OrderService;
using System.Security.Claims;

namespace SmartInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Get All
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return orders.IsSuccess ? Ok(orders) : BadRequest(orders);
        }

        // Get My Orders
        [HttpGet("MyOrders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var idString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(idString, out int userId))
                return Unauthorized("Invalid token");

            var orders = await _orderService.GetByUserIdAsync(userId);
            return orders.IsSuccess ? Ok(orders) : BadRequest(orders);
        }

        // Get By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var idString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(idString, out int userId))
                return Unauthorized("Invalid token");

            var order = await _orderService.GetByIdAsync(id, userId);
            return order.IsSuccess ? Ok(order) : BadRequest(order);
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto orderDto)
        {
            var idString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(idString, out int userId))
                return Unauthorized("Invalid token");

            var order = await _orderService.CreateAsync(orderDto, userId);
            return order.IsSuccess ? Ok(order) : BadRequest(order);
        }

        // Edit
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateOrderDto orderDto, int id)
        {
            var idString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(idString, out int userId))
                return Unauthorized("Invalid token");

            var order = await _orderService.UpdateAsync(orderDto, id, userId);
            return order.IsSuccess ? Ok(order) : BadRequest(order);
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var idString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(idString, out int userId))
                return Unauthorized("Invalid token");

            var order = await _orderService.CancelAsync(id, userId);
            return order.IsSuccess ? Ok(order) : BadRequest(order);
        }
    }
}
