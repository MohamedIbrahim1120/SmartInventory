using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly SmartInventoryDbContext _context;

        public ProductRepository(SmartInventoryDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToListAsync();
        }

        public IQueryable<Product> QueryWithInclude(params Expression<Func<Product, object>>[] includes)
        {
            IQueryable<Product> query = _context.Products;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }
    }
}
