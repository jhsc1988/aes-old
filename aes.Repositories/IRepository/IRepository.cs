using System.Linq.Expressions;

namespace aes.Repository.IRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        Task<int> Update(TEntity entity);
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindExact(Expression<Func<TEntity, bool>> predicate);
    }
}