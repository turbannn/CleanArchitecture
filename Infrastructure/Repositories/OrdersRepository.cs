using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly OrdersDbContext _ordersDbContext;
    private readonly ILogger<OrdersRepository> _logger;
    public OrdersRepository(OrdersDbContext ordersDbContext, ILogger<OrdersRepository> logger)
    {
        _ordersDbContext = ordersDbContext;
        _logger = logger;
    }


    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await _ordersDbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return order;
    }

    public async Task<bool> AddAsync(Order entityToAdd, CancellationToken cancellationToken)
    {
        try 
        {
            await _ordersDbContext.Orders.AddAsync(entityToAdd, cancellationToken);
            await _ordersDbContext.SaveChangesAsync();
            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Failed to add order {OrderId}", entityToAdd.Id);
            return false;
        }

    }
    public async Task<bool> UpdateAsync(Order entityToUpdate, CancellationToken cancellationToken)
    {
        try
        {
            var existingItem = await _ordersDbContext.Orders.FindAsync(entityToUpdate.Id, cancellationToken);
            if (existingItem == null)
            {
                _logger.LogWarning("Order {OrderId} not found for update", entityToUpdate.Id);
                return false; // Item not found
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load order {OrderId} for update", entityToUpdate.Id);
            return false;
        }

        try
        {
            await _ordersDbContext.Orders.Where(o => o.Id == entityToUpdate.Id).ExecuteUpdateAsync(s => s
                                    .SetProperty(p => p.ShippingAddress, entityToUpdate.ShippingAddress)
                                    .SetProperty(p => p.Notes, entityToUpdate.Notes));
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update order {OrderId}", entityToUpdate.Id);
            return false;
        }
    }
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _ordersDbContext.Orders.Where(o => o.Id == id).ExecuteDeleteAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete order {OrderId}", id);
            return false;
        }
    }
}
