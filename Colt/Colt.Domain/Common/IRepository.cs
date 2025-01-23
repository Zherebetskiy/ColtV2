namespace Colt.Domain.Common
{
    public interface IRepository<TEntity> where TEntity : BaseEntity<int>
    {
        Task<List<TEntity>> GetAsync(CancellationToken token);

        Task<TEntity> GetByIdAsync(int id, CancellationToken token);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken token);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken token);

        Task<bool> DeleteAsync(TEntity entity, CancellationToken token);

        Task<int> SaveChangesAsync(CancellationToken token);
    }
}
