using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class SmartInventoryDbContext : IdentityDbContext<ApplicationUser>
    {

        public SmartInventoryDbContext(DbContextOptions<SmartInventoryDbContext> options)
                : base(options)
            {
            }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<StockTransaction> StockTransactions { get; set; }

    }
}
