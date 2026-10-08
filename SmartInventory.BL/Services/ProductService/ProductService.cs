using SmartInventory.BL.Common;
using SmartInventory.BL.DTOs.Product;
using SmartInventory.DAL.Models;
using SmartInventory.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _repo;
        public ProductService(IProductRepo repo)
        {
            _repo = repo;
        }
        public async Task<ServiceResponse<int>> CreateAsync(CreateProductDto productDto)
        {
            if (productDto.Price < 0)
                return ServiceResponse<int>.Fail("Price cant be negative");
            
            if (productDto.Quantity < 0)
                return ServiceResponse<int>.Fail("Quantity cant be negative");

            var newProduct = new Product
            {
                Title = productDto.Title,
                Quantity = productDto.Quantity,
                Price = productDto.Price,
            };

            await _repo.CreateAsync(newProduct);
            await _repo.SaveChangesAsync();

            return ServiceResponse<int>.Success(newProduct.Id);
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                return ServiceResponse<bool>.Fail("Product not found!");

            await _repo.DeleteAsync(product);
            await _repo.SaveChangesAsync();

            return ServiceResponse<bool>.Success(true);
        }

        public async Task<ServiceResponse<List<GetProductDto>>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();
            if (products == null || !products.Any())
                return ServiceResponse<List<GetProductDto>>.Fail("No products yet");

            var productsDto = products.Select(x => new GetProductDto
            {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price,
                Quantity = x.Quantity
            }).ToList();

            return ServiceResponse<List<GetProductDto>>.Success(productsDto);
        }

        public async Task<ServiceResponse<GetProductDto>> GetByIdAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                return ServiceResponse<GetProductDto>.Fail("Product not found");

            var productDto = new GetProductDto
            {
                Id = id,
                Title = product.Title,
                Price = product.Price,
                Quantity = product.Quantity
            };

            return ServiceResponse<GetProductDto>.Success(productDto);
        }

        public async Task<ServiceResponse<PaginatedResult<GetProductDto>>> GetPaginatedAsync(int pageIndex, int pageSize)
        {
            var (products, totalCount) = await _repo.GetPaginatedAsync(pageIndex, pageSize);

            var productsDto = products.Select(x => new GetProductDto
            {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price,
                Quantity = x.Quantity
            }).ToList();

            var result = new PaginatedResult<GetProductDto>
            {
                Data = productsDto,
                PageNumber = pageIndex,
                PageSize = pageSize,
                totalRecords = totalCount
            };

            return ServiceResponse<PaginatedResult<GetProductDto>>.Success(result);
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(UpdateProductDto productDto, int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                return ServiceResponse<bool>.Fail("Product not found");

            if (product.Price < 0)
                return ServiceResponse<bool>.Fail("Price cant be negative");

            if (product.Quantity < 0)
                return ServiceResponse<bool>.Fail("Quantity cant be negative");

            product.Title = productDto.Title;
            product.Price = productDto.Price;
            product.Quantity = productDto.Quantity;

            await _repo.UpdateAsync(product);
            await _repo.SaveChangesAsync();

            return ServiceResponse<bool>.Success(true);
        }
    }
}
