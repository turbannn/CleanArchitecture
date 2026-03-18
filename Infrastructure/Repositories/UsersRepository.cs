using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly OrdersDbContext _dbContext;
    private readonly ILogger<UsersRepository> _logger;

    public UsersRepository(OrdersDbContext dbContext, ILogger<UsersRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
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
            _logger.LogError(ex, "Failed to add user {UserId}", entityToAdd.Id);
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
                _logger.LogWarning("User {UserId} not found for update", entityToUpdate.Id);
                return false; // Item not found
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load user {UserId} for update", entityToUpdate.Id);
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
            _logger.LogError(ex, "Failed to update user {UserId}", entityToUpdate.Id);
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
            _logger.LogError(ex, "Failed to delete user {UserId}", id);
            return false;
        }
    }
}
