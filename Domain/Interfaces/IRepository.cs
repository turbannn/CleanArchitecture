using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(TEntity entityToAdd, CancellationToken cancellationToken);
        Task UpdateAsync(TEntity entityToUpadte, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
