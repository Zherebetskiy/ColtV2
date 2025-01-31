using Colt.Domain.Common;
using Colt.Domain.Entities;

namespace Colt.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<string> GenerateInvoiceAsync(Customer customer, Order order, OrderDebtModel debt);
    }
}
