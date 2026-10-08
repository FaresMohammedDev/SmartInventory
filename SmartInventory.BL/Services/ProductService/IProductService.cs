using SmartInventory.BL.Common;
using SmartInventory.BL.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<GetProductDto>>> GetAllAsync();
        Task<ServiceResponse<GetProductDto>> GetByIdAsync(int id);
        Task<ServiceResponse<PaginatedResult<GetProductDto>>> GetPaginatedAsync(int pageIndex, int pageSize);
        Task<ServiceResponse<int>> CreateAsync(CreateProductDto productDto);
        Task<ServiceResponse<bool>> UpdateAsync(UpdateProductDto productDto, int id);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }
}
