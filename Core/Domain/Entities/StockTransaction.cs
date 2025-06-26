using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StockTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; } 
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string Type { get; set; } = null!;
        public string? Note { get; set; }
    }
}
