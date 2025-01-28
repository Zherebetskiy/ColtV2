using Colt.Domain.Entities;

namespace Colt.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetByIdAsync(int id);
        Task<List<Order>> GetAllAsync();
        Task<List<Order>> GetByCustomerIdAsync(int customerId);
        Task InsertAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<Order> DeliverAsync(Order order);
        Task<bool> DeleteAsync(int id);
    }
}
