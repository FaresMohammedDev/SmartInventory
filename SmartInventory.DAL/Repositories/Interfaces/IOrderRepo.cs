using SmartInventory.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Repositories.Interfaces
{
    public interface IOrderRepo : IGenericRepo<Order>
    {
        Task<Order?> GetOrderWithItemsAsync(int id);
        Task<List<Order>> GetAllWithItemsAsync();
        Task<(List<Order>, int totalCount)> GetPaginatedAsync(int userId, int pageIndex, int pageSize);
        Task<List<Order>> FindWithAsync(int id);
    }
}
