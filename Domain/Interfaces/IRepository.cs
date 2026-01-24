using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<TEntity?> GetAsync(Guid id);
        Task AddAsync(TEntity entityToAdd);
        Task UpdateAsync(TEntity entityToUpadte);
        Task DeleteAsync(Guid id);
    }
}
