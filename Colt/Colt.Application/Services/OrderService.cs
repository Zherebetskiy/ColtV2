using Colt.Application.Interfaces;
using Colt.Domain.Common;
using Colt.Domain.Entities;
using Colt.Domain.Repositories;

namespace Colt.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id, CancellationToken.None);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAsync(CancellationToken.None);
        }

        public async Task<PaginationModel<Order>> GetPaginatedAsync(int customerId, int skip, int take)
        {
            return await _orderRepository.GetPaginatedAsync(customerId, skip, take, CancellationToken.None);
        }

        public async Task<List<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await _orderRepository.GetByCustomerIdAsync(customerId, CancellationToken.None);
        }

        public async Task InsertAsync(Order order)
        {
            order.Products = order.Products
                .Where(x => (x.OrderedWeight.HasValue && x.OrderedWeight != 0) || (x.ActualWeight.HasValue && x.ActualWeight != 0))
                .ToList();

            await _orderRepository.AddAsync(order, CancellationToken.None);
        }

        public async Task<Order> DeliverAsync(Order order)
        {
            order.Status = Domain.Enums.OrderStatus.Delivered;

            return await _orderRepository.UpdateAsync(order, CancellationToken.None);
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            var addedProducts = order.Products.Where(x => x.Id == default).ToList();
            ////var updatedProducts = order.Products.Where(x => x.Id != default).ToList();
            var deletedProducts = order.Products.Where(x => x.Id != default && x.OrderedWeight == 0 && x.ActualWeight == 0).ToList();

            await _orderRepository.DeleteProductsAsync(deletedProducts, CancellationToken.None);
            
            foreach (var addedProduct in addedProducts)
            {
                addedProduct.Order = order;
                addedProduct.OrderId = order.Id;

                order.Products.Add(addedProduct);
            }

            return await _orderRepository.UpdateAsync(order, CancellationToken.None);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id, CancellationToken.None);

            return await _orderRepository.DeleteAsync(order, CancellationToken.None);
        }
    }
}
