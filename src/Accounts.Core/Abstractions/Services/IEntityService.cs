using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Accounts.Core.Domain;

namespace Accounts.Core.Abstractions.Services
{
    public interface IEntityService<T> : IService<T> where T: BaseEntity
    {
        Task<string> CreateAsync(T value);
        Task<IEnumerable<T>> GetAllAsync(params string[] filters);
        Task<T> GetByIdAsync(Guid id);
        Task<string> UpdateAsync(Guid id, T value);
        Task DeleteAsync(Guid id);
    }
}