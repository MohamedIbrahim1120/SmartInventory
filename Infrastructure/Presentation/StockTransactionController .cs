using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

[Route("api/[controller]")]
[ApiController]
public class StockTransactionController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StockTransactionController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var transactions = await _unitOfWork.StockTransactions.GetAllAsync();
        var dto = _mapper.Map<IEnumerable<StockTransactionDto>>(transactions);
        return Ok(dto);
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetByProductId(int productId)
    {
        var filtered = await _unitOfWork.StockTransactions.FindAsync(t => t.ProductId == productId);
        var dto = _mapper.Map<IEnumerable<StockTransactionDto>>(filtered);
        return Ok(dto);
    }
}
