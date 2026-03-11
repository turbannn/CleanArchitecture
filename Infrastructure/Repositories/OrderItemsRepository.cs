using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class OrderItemsRepository : IOrderItemsRepository
{
    private readonly OrdersDbContext _ordersDbContext;

    public OrderItemsRepository(OrdersDbContext ordersDbContext)
    {
        _ordersDbContext = ordersDbContext;
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
            // Log the exception (ex) here as needed
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
                return false; // Item not found
            }
        }
        catch (Exception ex)
        {
            // Log the exception (ex) here as needed
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
            // Log the exception (ex) here as needed
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
            // Log the exception (ex) here as needed
            return false;
        }
    }
}
