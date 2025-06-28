using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IGenericRepository<Category> Categories { get; }

        IGenericRepository<StockTransaction> StockTransactions { get; }

        IGenericRepository<Notification> Notifications { get; }

        IGenericRepository<Supplier> Suppliers { get; }

        Task<int> SaveChangesAsync();
    }

}
