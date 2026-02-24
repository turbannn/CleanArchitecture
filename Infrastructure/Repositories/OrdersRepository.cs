using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrdersDbContext _ordersDbContext;
        public OrdersRepository(OrdersDbContext ordersDbContext)
        {
            _ordersDbContext = ordersDbContext;
        }


        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _ordersDbContext.Orders.FindAsync(id, cancellationToken);
        }

        public async Task AddAsync(Order entityToAdd, CancellationToken cancellationToken)
        {
            await _ordersDbContext.Orders.AddAsync(entityToAdd, cancellationToken);
            await _ordersDbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Order entityToUpadte, CancellationToken cancellationToken)
        {
            await _ordersDbContext.Orders.ExecuteUpdateAsync(s => s.SetProperty(p => p.OrderDate, entityToUpadte.OrderDate));
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _ordersDbContext.Orders.Where(o => o.Id == id).ExecuteDeleteAsync(cancellationToken);
        }

    }
}
