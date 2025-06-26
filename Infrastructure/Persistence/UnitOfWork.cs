using Domain.Entities;
using Domain.Interfaces;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmartInventoryDbContext _context;

        private IProductRepository? _product;
        private IGenericRepository<Category>? _category;
        private IGenericRepository<StockTransaction>? _stockTransaction;

        public IProductRepository Products => _product ??= new ProductRepository(_context);
        public IGenericRepository<Category> Categories => _category ??= new GenericRepository<Category>(_context);
        public IGenericRepository<StockTransaction> StockTransactions => _stockTransaction ??= new GenericRepository<StockTransaction>(_context); // ✅ تم تفعيله

        public UnitOfWork(SmartInventoryDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
