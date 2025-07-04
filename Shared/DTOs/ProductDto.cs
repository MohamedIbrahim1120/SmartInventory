﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public Guid? SupplierId { get; set; }
        public string? SupplierName { get; set; }

    }
}
