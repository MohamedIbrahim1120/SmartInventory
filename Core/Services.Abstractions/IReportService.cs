using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IReportService
    {
        Task<int> GetTotalProductCountAsync();
        Task<int> GetTotalQuantityAsync();
        Task<int> GetLowStockCountAsync(int threshold = 5);
        Task<Dictionary<string, int>> GetProductCountPerCategoryAsync();

    }
}
