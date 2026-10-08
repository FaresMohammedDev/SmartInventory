using Microsoft.EntityFrameworkCore;
using SmartInventory.DAL.Data;
using SmartInventory.DAL.Models;
using SmartInventory.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Repositories.Implementations
{
    public class OrderRepo : GenericRepo<Order>, IOrderRepo
    {
        private readonly ApplicationDbContext _context;
        public OrderRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderWithItemsAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetAllWithItemsAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<(List<Order>, int totalCount)> GetPaginatedAsync(int userId, int pageIndex, int pageSize)
        {
            var queue = _context.Orders.Where(x => x.userId == userId).OrderBy(x => x.TotalPrice);

            var totalCount = queue.Count();

            var orders = await queue
                .Include(x => x.User)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (orders, totalCount);
        }

        public async Task<List<Order>> FindWithAsync(int userId)
        {
            return await _context.Orders
                .Where(x => x.userId == userId)
                .Include(x => x.User)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ToListAsync();
                
        }

    }
}
