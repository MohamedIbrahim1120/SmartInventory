using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class InventorySuggestionDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public string Suggestion { get; set; } = "";
    }
}
