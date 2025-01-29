using Colt.Domain.Common;
using Colt.Domain.Entities;
using Colt.Domain.Repositories;
using Colt.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Colt.Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly DbSet<Payment> _dbSet;

        public PaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.GetSet<Payment>();
        }

        public async Task<bool> DeletePaymentsAsync(List<Payment> payments, CancellationToken cancellationToken)
        {
            _dbSet.RemoveRange(payments);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<PaginationModel<Payment>> GetPaginatedAsync(int customerId, int skip, int take, CancellationToken cancellationToken)
        {
            var count = await _dbSet
                .Where(x => x.CustomerId == customerId)
                .CountAsync(cancellationToken);

            var result = await _dbSet
                .Where(x => x.CustomerId == customerId)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);

            return new PaginationModel<Payment>
            {
                Collection = result,
                TotalCount = count
            };
        }

        public Task<List<Payment>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return _dbSet
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.Date)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
