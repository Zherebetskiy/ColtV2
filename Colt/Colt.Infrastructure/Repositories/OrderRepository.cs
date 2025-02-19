﻿using Colt.Domain.Common;
using Colt.Domain.Entities;
using Colt.Domain.Repositories;
using Colt.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Colt.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly DbSet<Order> _dbSet;

        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.GetSet<Order>();
        }

        public Task<List<OrderProduct>> GetStatisticsAsync(int? customerId, string productName, DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var query = _dbContext.GetSet<OrderProduct>()
                .Include(x => x.Order)
                .Where(x => x.Order.Delivery >= from && x.Order.Delivery <= to && x.Order.Status == Domain.Enums.OrderStatus.Delivered);

            if (customerId.HasValue)
            {
                query = query.Where(x => x.Order.CustomerId == customerId);
            }

            if (!string.IsNullOrWhiteSpace(productName))
            {
                query = query.Where(x => x.ProductName == productName);
            }

            return query
                .OrderBy(x => x.ProductName)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<PaginationModel<Order>> GetPaginatedAsync(int customerId, int skip, int take, CancellationToken cancellationToken)
        {
            var count = await _dbSet
                .Where(x => x.CustomerId == customerId)
                .CountAsync(cancellationToken);

            var result = await _dbSet
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.Date)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);

            return new PaginationModel<Order>
            {
                Collection = result,
                TotalCount = count
            };
        }

        public override Task<Order> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _dbSet
                .Where(x => x.Id == id)
                .Include(x => x.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<Order> GetByIdWithCustomerAsync(int id, CancellationToken cancellationToken)
        {
            return _dbSet
                .Where(x => x.Id == id)
                .Include(x => x.Products.Where(x => x.TotalPrice.HasValue))
                .Include(x => x.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        public override Task<List<Order>> GetAsync(CancellationToken cancellationToken)
        {
            return _dbSet
                .Include(x => x.Customer)
                .OrderByDescending(x => x.Date)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public Task<List<Order>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return _dbSet
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.Date)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public Task<List<OrderProduct>> GetProductsAsync(int orderId, CancellationToken cancellationToken)
        {
            return _dbContext.GetSet<OrderProduct>()
                .Where(x => x.OrderId == orderId)
                .OrderBy(x => x.ProductName)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> DeleteProductsAsync(List<OrderProduct> products, CancellationToken cancellationToken)
        {
            _dbContext.GetSet<OrderProduct>().RemoveRange(products);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
