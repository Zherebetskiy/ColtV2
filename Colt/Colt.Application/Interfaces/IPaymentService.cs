using Colt.Domain.Entities;

namespace Colt.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<List<Payment>> GetByCustomerIdAsync(int customerId);
    }
}
