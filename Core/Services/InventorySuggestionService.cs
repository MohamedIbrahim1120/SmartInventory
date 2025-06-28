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
    public class InventorySuggestionService : IInventorySuggestionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventorySuggestionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<InventorySuggestionDto>> GetLowStockSuggestionsAsync()
        {
            var products = await _unitOfWork.Products
                .FindAsync(p => p.Quantity <= p.ReorderThreshold);

            return products.Select(p => new InventorySuggestionDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                Suggestion = $"The product {p.Name} is low in stock. Consider reordering soon."
            }).ToList();
        }

        public async Task<List<InventorySuggestionDto>> GetStagnantProductSuggestionsAsync()
        {
            var thresholdDate = DateTime.UtcNow.AddMonths(-3);

            var allProducts = await _unitOfWork.Products.GetAllAsync();

            var stagnant = new List<Product>();

            foreach (var p in allProducts)
            {
                var hasMovement = await _unitOfWork.StockTransactions
                    .ExistsAsync(t => t.ProductId == p.Id && t.Timestamp > thresholdDate);

                if (!hasMovement)
                    stagnant.Add(p);
            }

            return stagnant.Select(p => new InventorySuggestionDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                Suggestion = $"The product {p.Name} has had no sales in the last 3 months. Consider offering a discount."
            }).ToList();
        }

        public async Task<List<InventorySuggestionDto>> GetFastMovingSuggestionsAsync()
        {
            var thresholdDate = DateTime.UtcNow.AddMonths(-1);

            var transactions = await _unitOfWork.StockTransactions
                .FindAsync(t => t.Type == "out" && t.Timestamp > thresholdDate);

            var grouped = transactions
                .GroupBy(t => t.ProductId)
                .OrderByDescending(g => g.Count())
                .Take(5);

            var suggestions = new List<InventorySuggestionDto>();

            foreach (var group in grouped)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(group.Key);
                if (product != null)
                {
                    suggestions.Add(new InventorySuggestionDto
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Suggestion = $"The product {product.Name} is selling quickly. Consider increasing stock or promoting it further."
                    });
                }
            }

            return suggestions;
        }
    }
}
