using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.BL.DTOs.Product;
using SmartInventory.BL.Services.ProductService;
using SmartInventory.DAL.Models;

namespace SmartInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        // Get All
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllAsync();
            return products.IsSuccess ? Ok(products) : BadRequest(products);
        }

        // Get By Id
        [HttpGet("id")]
        public async Task<IActionResult> GetbyId(int id)
        {
            var product = await _service.GetByIdAsync(id);
            return product.IsSuccess ? Ok(product) : BadRequest(product);
        }

        // Get Paginated
        [HttpGet("Paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 5)
        {
            var products = await _service.GetPaginatedAsync(pageIndex, pageSize);
            return products.IsSuccess ? Ok(products) : BadRequest(products);
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto productDto)
        {
            var product = await _service.CreateAsync(productDto);
            return product.IsSuccess ? Ok(product) : BadRequest(product);
        }

        // Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateProductDto productDto, int id)
        {
            var product = await _service.UpdateAsync(productDto, id);
            return product.IsSuccess ? Ok(product) : BadRequest(product);
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _service.DeleteAsync(id);
            return product.IsSuccess ? Ok(product) : BadRequest(product);
        }
    }
}
