using Colt.Domain.Common;
using Colt.Domain.Entities;
using Colt.Domain.Repositories;
using Colt.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Colt.Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly DbSet<Customer> _dbSet;

        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.GetSet<Customer>();
        }

        public override Task<Customer> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _dbSet
                .Where(x => x.Id == id)
                .Include(x => x.Products)
                .Include(x => x.Payments)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<Customer> GetWithProductsAsync(int id, CancellationToken cancellationToken)
        {
            return _dbSet
                .Where(x => x.Id == id)
                .Include(x => x.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> DeleteProductsAsync(List<CustomerProduct> products, CancellationToken cancellationToken)
        {
            _dbContext.GetSet<CustomerProduct>().RemoveRange(products);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public Task<List<CustomerProduct>> GetProductsByIdAsync(int productId, CancellationToken cancellationToken)
        {
            return _dbContext.GetSet<CustomerProduct>()
                .Where(x => x.ProductId == productId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public Task<List<CustomerProduct>> GetProductsByCustomerIdAsync(int id, CancellationToken cancellationToken)
        {
            return _dbContext.GetSet<CustomerProduct>()
                .Where(x => x.CustomerId == id)
                .Include(x => x.Product)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
