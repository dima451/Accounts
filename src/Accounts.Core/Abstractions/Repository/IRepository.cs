using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Accounts.Core.Domain;

namespace Accounts.Core.Abstractions.Repository
{
    public interface IRepository<T> where T: BaseEntity
    {
        public Task<T> GetByIdAsync(Guid id);
        public Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetWithIncludeAsync(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties);
        public Task<T> UpdateAsync(Guid id, T value);
        public Task CreateAsync(T value);
        public Task DeleteAsync(Guid id);
    }
}