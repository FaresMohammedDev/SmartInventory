using SmartInventory.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Repositories.Interfaces
{
    public interface IProductRepo : IGenericRepo<Product>
    {
        Task<(List<Product>, int totalCount)> GetPaginatedAsync(int pageIndex, int pageSize);
    }
}
