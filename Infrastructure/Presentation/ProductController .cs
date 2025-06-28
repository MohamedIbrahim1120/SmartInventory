using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DTOs;

namespace SmartInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IProductService productService,IUnitOfWork unitOfWork)
        {
            _productService = productService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            var created = await _productService.AddAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto dto)
        {
            var result = await _productService.UpdateAsync(id, dto);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] int? categoryId)
        {
            var result = await _productService.SearchAsync(name, categoryId);
            return Ok(result);
        }


        [HttpGet("Paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var (data, totalCount) = await _productService.GetPagedAsync(page, size);
            return Ok(new { totalCount, data });
        }

        [HttpGet("by-category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var products = await _unitOfWork.Products.FindAsync(p => p.CategoryId == categoryId);

            var result = products.Select(p => new
            {
                p.Id,
                p.Name,
                p.Quantity,
                p.Price,
                p.CategoryId
            });

            return Ok(result);
        }

        [HttpGet("WithSuppliers")]
        public async Task<IActionResult> GetProductsWithSuppliers()
        {
            var products = await _unitOfWork.Products.GetAllAsync();

            var result = products.Select(p => new ProductWithSupplierDto
            {
                Id = p.Id,
                Name = p.Name,
                Quantity = p.Quantity,
                CategoryId = p.CategoryId ?? 0,
                CategoryName = p.Category?.Name ?? "WithOut Category",
                SupplierId = p.SupplierId,
                SupplierName = p.Supplier?.Name ?? "WithOut Suppliers"
            }).ToList();

            return Ok(result);
        }

    }

}

