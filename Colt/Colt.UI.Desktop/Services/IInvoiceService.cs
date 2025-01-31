using Colt.Domain.Common;
using Colt.Domain.Entities;

namespace Colt.UI.Desktop
{
    public interface IInvoiceService
    {
        Task<string> GenerateInvoiceAsync(Customer customer, Order order, OrderDebtModel debt);
    }
}
