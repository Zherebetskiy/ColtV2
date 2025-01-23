using Colt.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Colt.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity<int>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.GetSet<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken token)
        {
            await _dbSet.AddAsync(entity, token);

            await _dbContext.SaveChangesAsync(token);

            return entity;
        }

        public virtual Task<List<TEntity>> GetAsync(CancellationToken token)
        {
            return _dbSet
                .AsNoTracking()
                .ToListAsync(token);
        }

        public virtual async Task<TEntity> GetByIdAsync(int id, CancellationToken token)
        {
            var entity = await _dbSet.FindAsync(id, token);

            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken token)
        {
            _dbContext.GetSet<TEntity>().Update(entity);

            await _dbContext.SaveChangesAsync(token);

            return entity;
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity, CancellationToken token)
        {
            try
            {
                _dbSet.Remove(entity);

                await _dbContext.SaveChangesAsync(token);

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual Task<int> SaveChangesAsync(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync(token);
        }

    }
}
