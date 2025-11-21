using Hotel.Entities.Entities;
using Hotel.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Hotel.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly BookingDBContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(BookingDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id) =>
            await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<List<TEntity>> GetAllAsync() =>
            await _dbSet.Where(x => !x.IsDeleted).ToListAsync();

        public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);
        public void Update(TEntity entity) => _dbSet.Update(entity);

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return;
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities) =>
            await _dbSet.AddRangeAsync(entities);

        public void UpdateRange(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

        public async Task SoftDeleteRangeAsync(IEnumerable<Guid> ids)
        {
            var entities = await _dbSet.Where(e => ids.Contains(e.Id)).ToListAsync();
            foreach (var e in entities)
            {
                e.IsDeleted = true;
            }
            _dbSet.UpdateRange(entities);
        }
        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate) =>
            await _dbSet.Where(predicate).Where(x => !x.IsDeleted).ToListAsync();

        public IQueryable<TEntity> Query() => _dbSet.Where(x => !x.IsDeleted).AsQueryable();
    }
}
