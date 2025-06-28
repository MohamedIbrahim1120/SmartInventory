using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IInventorySuggestionService
    {
        Task<List<InventorySuggestionDto>> GetLowStockSuggestionsAsync();
        Task<List<InventorySuggestionDto>> GetStagnantProductSuggestionsAsync();
        Task<List<InventorySuggestionDto>> GetFastMovingSuggestionsAsync();
    }
}
