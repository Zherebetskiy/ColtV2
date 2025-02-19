﻿using Colt.Domain.Common;
using Colt.Domain.Entities;

namespace Colt.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetByIdAsync(int id);
        Task<PaginationModel<Order>> GetPaginatedAsync(int customerId, int skip, int take);
        Task<List<Order>> GetAllAsync();
        Task<List<Order>> GetByCustomerIdAsync(int customerId);
        Task InsertAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<Order> DeliverAsync(Order order);
        Task<bool> DeleteAsync(int id);
        Task<List<OrderProduct>> GetStatisticsAsync(int? customerId, string productName, DateTime from, DateTime to);
    }
}
