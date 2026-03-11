using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly OrdersDbContext _ordersDbContext;
    public OrdersRepository(OrdersDbContext ordersDbContext)
    {
        _ordersDbContext = ordersDbContext;
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
            // Log the exception (ex) here as needed
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
            await _ordersDbContext.Orders.Where(o => o.Id == entityToUpdate.Id).ExecuteUpdateAsync(s => s
                                    .SetProperty(p => p.ShippingAddress, entityToUpdate.ShippingAddress)
                                    .SetProperty(p => p.Notes, entityToUpdate.Notes));
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
            await _ordersDbContext.Orders.Where(o => o.Id == id).ExecuteDeleteAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            // Log the exception (ex) here as needed
            return false;
        }
    }
}
