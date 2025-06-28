using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

[ApiController]
[Route("api/[controller]")]
public class SupplierController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SupplierController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var suppliers = await _unitOfWork.Suppliers.GetAllAsync();
        return Ok(_mapper.Map<List<SupplierDto>>(suppliers));
    }

    [HttpPost]
    public async Task<IActionResult> Create(SupplierDto dto)
    {
        var supplier = _mapper.Map<Supplier>(dto);
        await _unitOfWork.Suppliers.AddAsync(supplier);
        await _unitOfWork.SaveChangesAsync();
        return Ok(_mapper.Map<SupplierDto>(supplier));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, SupplierDto dto)
    {
        var existing = await _unitOfWork.Suppliers.GetByIdAsync(id);
        if (existing == null) return NotFound();

        _mapper.Map(dto, existing);
        _unitOfWork.Suppliers.Update(existing);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);
        if (supplier == null) return NotFound();

        _unitOfWork.Suppliers.Delete(supplier);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}
