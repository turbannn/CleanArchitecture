using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly OrdersDbContext _ordersDbContext;

        public OrderItemsRepository(OrdersDbContext ordersDbContext)
        {
            _ordersDbContext = ordersDbContext;
        }

        public async Task<OrderItem?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await _ordersDbContext.OrderItems.FindAsync(id, cancellationToken);
            return item;
        }
        public async Task AddAsync(OrderItem entityToAdd, CancellationToken cancellationToken)
        {
            await _ordersDbContext.OrderItems.AddAsync(entityToAdd, cancellationToken);
            await _ordersDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(OrderItem entityToUpadte, CancellationToken cancellationToken)
        {
            await _ordersDbContext.OrderItems.ExecuteUpdateAsync(
                s => s
                .SetProperty(p => p.UnitPrice, entityToUpadte.UnitPrice)
                .SetProperty(p => p.ProductName, entityToUpadte.ProductName)
                .SetProperty(p => p.Quantity, entityToUpadte.Quantity), 
                cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _ordersDbContext.OrderItems.Where(oi => oi.Id == id).ExecuteDeleteAsync(cancellationToken);
        }
    }
}
