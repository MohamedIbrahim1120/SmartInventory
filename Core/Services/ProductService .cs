using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Services.Abstractions;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public  class ProductService : IProductService
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
            var products = await _unitOfWork.Products.FindAsyncWithInclude(p => true, p => p.Category);
            return _mapper.Map<IEnumerable<ProductDto>>(products);

        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = (await _unitOfWork.Products.FindAsyncWithInclude(p => p.Id == id, p => p.Category)).FirstOrDefault();
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> AddAsync(ProductDto dto)
        {
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
                p => p.Category
            );

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<(IEnumerable<ProductDto> data, int totalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var all = await _unitOfWork.Products.FindAsyncWithInclude(p => true, p => p.Category);
            var paged = all.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return (_mapper.Map<IEnumerable<ProductDto>>(paged), all.Count());
        }


    }
}