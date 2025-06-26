using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetStats()
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        var categories = await _unitOfWork.Categories.GetAllAsync();
        var lowStock = products.Count(p => p.Quantity <= 5);

        return Ok(new
        {
            TotalProducts = products.Count(),
            TotalCategories = categories.Count(),
            LowStockCount = lowStock
        });
    }
}
