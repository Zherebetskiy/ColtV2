using Colt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Colt.Domain.Common
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; set; }

        DbSet<Product> Products { get; set; }

        DbSet<CustomerProduct> CustomerProducts { get; set; }

        DbSet<Order> Orders { get; set; }
        DbSet<Payment> Payments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DbSet<TEntity> GetSet<TEntity>() where TEntity : BaseEntity<int>;

        EntityEntry GetEntry(object entity);

        ChangeTracker ChangeTracker { get; }
    }
}
