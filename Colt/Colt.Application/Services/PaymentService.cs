using Colt.Application.Interfaces;
using Colt.Domain.Common;
using Colt.Domain.Entities;
using Colt.Domain.Repositories;

namespace Colt.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public Task<List<Payment>> GetByCustomerIdAsync(int customerId)
        {
            return _paymentRepository.GetByCustomerIdAsync(customerId, CancellationToken.None);
        }

        public async Task<PaginationModel<Payment>> GetPaginatedAsync(int customerId, int skip, int take)
        {
            return await _paymentRepository.GetPaginatedAsync(customerId, skip, take, CancellationToken.None);
        }
    }
}
