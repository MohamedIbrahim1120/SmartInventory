using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class StockTransactionDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; } = null!;
        public string? Note { get; set; }
    }
}
