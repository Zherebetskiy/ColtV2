using Colt.Domain.Common;
using Colt.Domain.Entities;

namespace Colt.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<bool> DeleteProductsAsync(List<OrderProduct> products, CancellationToken cancellationToken);
        Task<List<Order>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<Order> GetByIdWithCustomerAsync(int id, CancellationToken cancellationToken);
        Task<PaginationModel<Order>> GetPaginatedAsync(int customerId, int skip, int take, CancellationToken cancellationToken);
        Task<List<OrderProduct>> GetProductsAsync(int orderId, CancellationToken cancellationToken);
        Task<List<OrderProduct>> GetStatisticsAsync(int? customerId, string productName, DateTime from, DateTime to, CancellationToken cancellationToken);

    }
}
