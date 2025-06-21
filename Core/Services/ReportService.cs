using Domain.Interfaces;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetTotalProductCountAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return products.Count();
        }

        public async Task<int> GetTotalQuantityAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return products.Sum(p => p.Quantity);
        }
        public async Task<int> GetLowStockCountAsync(int threshold = 5)
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return products.Count(p => p.Quantity < threshold);
        }
        public async Task<Dictionary<string, int>> GetProductCountPerCategoryAsync()
        {
            var products = await _unitOfWork.Products.FindAsyncWithInclude(p => true, p => p.Category);

            return products
                .Where(p => p.Category != null)
                .GroupBy(p => p.Category!.Name)
                .ToDictionary(g => g.Key, g => g.Count());
        }


    }
}
