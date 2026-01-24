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

        public async Task<OrderItem?> GetAsync(Guid id)
        {
            var item = await _ordersDbContext.OrderItems.FindAsync(id);
            return item;
        }
        public async Task AddAsync(OrderItem entityToAdd)
        {
            await _ordersDbContext.OrderItems.AddAsync(entityToAdd);
            await _ordersDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderItem entityToUpadte)
        {
            await _ordersDbContext.OrderItems.ExecuteUpdateAsync(
                s => s
                .SetProperty(p => p.UnitPrice, entityToUpadte.UnitPrice)
                .SetProperty(p => p.ProductName, entityToUpadte.ProductName)
                .SetProperty(p => p.Quantity, entityToUpadte.Quantity)
            );
        }

        public async Task DeleteAsync(Guid id)
        {
            await _ordersDbContext.OrderItems.Where(oi => oi.Id == id).ExecuteDeleteAsync();
        }
    }
}
