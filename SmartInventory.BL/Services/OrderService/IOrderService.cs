using SmartInventory.BL.Common;
using SmartInventory.BL.DTOs.Order;
using SmartInventory.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<List<GetOrderDto>>> GetAllAsync();
        Task<ServiceResponse<List<GetOrderDto>>> GetByUserIdAsync(int userId);
        Task<ServiceResponse<PaginatedResult<GetOrderDto>>> GetPaginatedAsync(int userId, int pageIndex, int pageSize);
        Task<ServiceResponse<GetOrderDto>> GetByIdAsync(int id, int userId);
        Task<ServiceResponse<int>> CreateAsync(CreateOrderDto orderDto, int userId);
        Task<ServiceResponse<bool>> UpdateAsync(UpdateOrderDto orderDto, int id, int userId);
        Task<ServiceResponse<bool>> CancelAsync(int id, int userId);
    }
}
