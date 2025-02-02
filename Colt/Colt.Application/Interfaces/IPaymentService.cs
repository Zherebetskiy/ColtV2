using Colt.Domain.Common;
using Colt.Domain.Entities;

namespace Colt.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<List<Payment>> GetByCustomerIdAsync(int customerId);
        Task<List<Payment>> GetStatisticsAsync(int? customerId, DateTime from, DateTime to);
        Task<PaginationModel<Payment>> GetPaginatedAsync(int customerId, int skip, int take);
    }
}
