using System.Linq.Expressions;
using Domain.CVS.Domain;

namespace Application.Common.Interfaces.CVS
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        TEntity GetByID(object id);

        void Insert(TEntity entity);

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", CancellationToken cancellationToken = default);

        Task<TEntity> GetByIDAsync(object id, CancellationToken cancellationToken = default);

        Task<TEntity> GetByIDAsync(object id, string includeProperties = "", CancellationToken cancellationToken = default);

        Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(object id, CancellationToken cancellationToken = default);

        Task DeleteAsync(TEntity entityToDelete, CancellationToken cancellationToken = default);

        Task UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken = default);

        Task<Organization> GetByKVKNumberAsync(string kvkNumber, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> FullTextSearch(string searchTerm, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] properties);
    }
}
