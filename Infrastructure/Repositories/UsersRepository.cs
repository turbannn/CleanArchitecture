using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly OrdersDbContext _dbContext;

    public UsersRepository(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Orders)
            .ThenInclude(o => o.Items)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return user;
    }

    public async Task<bool> AddAsync(User entityToAdd, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Users.AddAsync(entityToAdd, cancellationToken);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Log the exception (ex) here as needed
            return false;
        }
    }
   
    public async Task<bool> UpdateAsync(User entityToUpdate, CancellationToken cancellationToken)
    {
        try
        {
            var existingItem = await _dbContext.Users.FindAsync(entityToUpdate.Id, cancellationToken);
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
            await _dbContext.Users.Where(u => u.Id == entityToUpdate.Id).ExecuteUpdateAsync(s => s
                            .SetProperty(u => u.Username, entityToUpdate.Username)
                            .SetProperty(u => u.Password, entityToUpdate.Password));
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
            await _dbContext.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Log the exception (ex) here as needed
            return false;
        }
    }
}
