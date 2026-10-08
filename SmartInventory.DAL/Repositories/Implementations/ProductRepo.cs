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
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        private readonly ApplicationDbContext _context;
        public ProductRepo(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }

        public async Task<(List<Product>, int totalCount)> GetPaginatedAsync(int pageIndex, int pageSize)
        {
            var queue = await _context.Products.ToListAsync();

            var totalCounts = queue.Count();

            var products = queue
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (products, totalCounts);
        }
    }
}
