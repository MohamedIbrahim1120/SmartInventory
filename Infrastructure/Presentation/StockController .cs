using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

[Route("api/[controller]")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StockController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> RecordStock([FromBody] StockTransactionDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
        if (product == null)
            return NotFound("Product not found");

        if (dto.Type.ToLower() == "in")
            product.Quantity += dto.Quantity;
        else if (dto.Type.ToLower() == "out")
        {
            if (product.Quantity < dto.Quantity)
                return BadRequest("Not enough stock");
            product.Quantity -= dto.Quantity;
        }
        else
        {
            return BadRequest("Invalid transaction type (must be 'in' or 'out')");
        }

        var transaction = _mapper.Map<StockTransaction>(dto);
        await _unitOfWork.StockTransactions.AddAsync(transaction);
        _unitOfWork.Products.Update(product);

        await _unitOfWork.SaveChangesAsync();

        return Ok("Stock updated successfully ");
    }
}
