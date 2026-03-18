using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class OrderItemsRepository : IOrderItemsRepository
{
    private readonly OrdersDbContext _ordersDbContext;
    private readonly ILogger<OrderItemsRepository> _logger;

    public OrderItemsRepository(OrdersDbContext ordersDbContext, ILogger<OrderItemsRepository> logger)
    {
        _ordersDbContext = ordersDbContext;
        _logger = logger;
    }

    public async Task<OrderItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var item = await _ordersDbContext.OrderItems.FindAsync(id, cancellationToken);
        return item;
    }
    public async Task<bool> AddAsync(OrderItem entityToAdd, CancellationToken cancellationToken)
    {
        try
        {
            await _ordersDbContext.OrderItems.AddAsync(entityToAdd, cancellationToken);
            await _ordersDbContext.SaveChangesAsync(cancellationToken);
            return true;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add order item {OrderItemId}", entityToAdd.Id);
            return false;
        }
    }

    public async Task<bool> UpdateAsync(OrderItem entityToUpdate, CancellationToken cancellationToken)
    {
        try
        {
            var existingItem = await _ordersDbContext.OrderItems.FindAsync(entityToUpdate.Id, cancellationToken);
            if (existingItem == null)
            {
                _logger.LogWarning("Order item {OrderItemId} not found for update", entityToUpdate.Id);
                return false; // Item not found
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load order item {OrderItemId} for update", entityToUpdate.Id);
            return false;
        }

        try
        {
            await _ordersDbContext.OrderItems.Where(oi => oi.Id == entityToUpdate.Id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.UnitPrice, entityToUpdate.UnitPrice)
                .SetProperty(p => p.ProductName, entityToUpdate.ProductName)
                .SetProperty(p => p.Quantity, entityToUpdate.Quantity),
                cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update order item {OrderItemId}", entityToUpdate.Id);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _ordersDbContext.OrderItems.Where(oi => oi.Id == id).ExecuteDeleteAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete order item {OrderItemId}", id);
            return false;
        }
    }
}
