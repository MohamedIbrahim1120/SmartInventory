using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace SmartInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LowStockReportController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LowStockReportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetLowStockProducts([FromQuery] int threshold = 5)
        {
            var products = await _unitOfWork.Products.FindAsync(p => p.Quantity <= threshold);

            var result = products.Select(p => new
            {
                p.Id,
                p.Name,
                p.Quantity,
                p.Price
            });

            return Ok(result);
        }
    }
}
