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
