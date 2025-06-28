using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Abstractions;
using Shared.DTOs;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.FindAsyncWithInclude(
                p => true,
                p => p.Category,
                p => p.Supplier
            );

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = (await _unitOfWork.Products.FindAsyncWithInclude(
                p => p.Id == id,
                p => p.Category,
                p => p.Supplier
            )).FirstOrDefault();

            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> AddAsync(ProductDto dto)
        {
            if (dto.SupplierId.HasValue)
            {
                var supplierExists = await _unitOfWork.Suppliers.ExistsAsync(s => s.Id == dto.SupplierId);
                if (!supplierExists)
                    throw new Exception("Supplier not found.");
            }

            var product = _mapper.Map<Product>(dto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> UpdateAsync(int id, ProductDto dto)
        {
            var existing = await _unitOfWork.Products.GetByIdAsync(id);
            if (existing == null)
                return false;

            if (dto.SupplierId.HasValue)
            {
                var supplierExists = await _unitOfWork.Suppliers.ExistsAsync(s => s.Id == dto.SupplierId);
                if (!supplierExists)
                    throw new Exception("Supplier not found.");
            }

            _mapper.Map(dto, existing);
            _unitOfWork.Products.Update(existing);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return false;

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProductDto>> SearchAsync(string? name, int? categoryId)
        {
            var products = await _unitOfWork.Products.FindAsyncWithInclude(
                p => (string.IsNullOrEmpty(name) || p.Name.Contains(name)) &&
                     (!categoryId.HasValue || p.CategoryId == categoryId),
                p => p.Category,
                p => p.Supplier
            );

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<(IEnumerable<ProductDto> data, int totalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _unitOfWork.Products.QueryWithInclude(p => p.Category, p => p.Supplier);
            var totalCount = await query.CountAsync();
            var pagedData = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (_mapper.Map<IEnumerable<ProductDto>>(pagedData), totalCount);
        }
    }
}
