using Colt.Domain.Common;
using Colt.Domain.Entities;

namespace Colt.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllAsync();
        Task InsertAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(int id);
        Task<List<CustomerProduct>> GetProductsAsync(int customerId);
        Task<OrderDebtModel> GetDebtAsync(int id);
    }
}
