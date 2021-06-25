using Accounts.Core.Abstractions.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Accounts.Core.Domain;
using Accounts.DataAcess.Data;
using Microsoft.EntityFrameworkCore;

namespace Accounts.DataAcess.Repositories
{
    public class EFRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _dbSet;
        private readonly ApplicationContext _dbContext;

        public EFRepository(ApplicationContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<T>();
        }

        public async Task CreateAsync(T value)
        {
            await _dbSet.AddAsync(value);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await _dbSet.FindAsync(id);

            _dbSet.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var values = await _dbSet.AsNoTracking().ToListAsync();

            return values;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var value = await _dbSet.FindAsync(id);

            return value;
        }

        public Task<IEnumerable<T>> GetWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            return Task.FromResult(Include(includeProperties).AsEnumerable());
        }

        public Task<IEnumerable<T>> GetWithIncludeAsync(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return Task.FromResult(query.Where(predicate).AsEnumerable());
        }

        public async Task<T> UpdateAsync(Guid id, T value)
        {
            try
            {
                var entry = await _dbSet.AsNoTracking().FirstAsync(c=>c.Id == id);//.FindAsync(id);
                value.Id = id;
                entry = value;

                _dbSet.Update(entry);

                await _dbContext.SaveChangesAsync();

                return entry;
            }
            catch (Exception e)
            {
                return await Task.FromException<T>(e);
            }
        }

        private IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            return includeProperties
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}