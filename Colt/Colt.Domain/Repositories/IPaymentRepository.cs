using Colt.Domain.Common;
using Colt.Domain.Entities;

namespace Colt.Domain.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<bool> DeletePaymentsAsync(List<Payment> products, CancellationToken cancellationToken);

        Task<List<Payment>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
    }
}
