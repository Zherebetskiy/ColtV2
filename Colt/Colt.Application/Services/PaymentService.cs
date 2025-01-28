using Colt.Application.Interfaces;
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
    }
}
