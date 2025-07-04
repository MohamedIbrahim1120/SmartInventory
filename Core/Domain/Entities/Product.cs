﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; } // Primary Key

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int ReorderThreshold { get; set; } = 5;

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
    }
}
