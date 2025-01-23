using Colt.Domain.Common;
using Colt.Domain.Entities;

namespace Colt.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetWithProductsAsync(int id, CancellationToken cancellationToken);

        Task<bool> DeleteProductsAsync(List<CustomerProduct> products, CancellationToken cancellationToken);

        Task<List<CustomerProduct>> GetProductsByIdAsync(int productId, CancellationToken cancellationToken);

        Task<List<CustomerProduct>> GetProductsByCustomerIdAsync(int id, CancellationToken cancellationToken);
    }
}
