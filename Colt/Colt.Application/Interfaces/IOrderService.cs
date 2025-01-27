using Colt.Domain.Entities;

namespace Colt.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetByIdAsync(int id);
    }
}
