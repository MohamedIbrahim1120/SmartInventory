using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace SmartInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("TotalProducts")]
        public async Task<IActionResult> GetTotalProductCount()
        {
            var count = await _reportService.GetTotalProductCountAsync();
            return Ok(new { totalProductCount = count });
        }

        [HttpGet("TotalQuantity")]
        public async Task<IActionResult> GetTotalQuantity()
        {
            var total = await _reportService.GetTotalQuantityAsync();
            return Ok(new { totalQuantity = total });
        }

        [HttpGet("LowStock")]
        public async Task<IActionResult> GetLowStockCount([FromQuery] int threshold = 5)
        {
            var count = await _reportService.GetLowStockCountAsync(threshold);
            return Ok(new { lowStockCount = count });
        }

        [HttpGet("ProductCountPerCategory")]
        public async Task<IActionResult> GetProductCountPerCategory()
        {
            var result = await _reportService.GetProductCountPerCategoryAsync();
            return Ok(result);
        }



    }
}
