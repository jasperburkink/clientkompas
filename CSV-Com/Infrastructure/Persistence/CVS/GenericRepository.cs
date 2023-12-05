using System.Linq.Expressions;
using Application.Common.Interfaces.CVS;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.CVS
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        public CVSDbContext Context { get; }

        internal DbSet<TEntity> _dbSet;

        public GenericRepository(CVSDbContext context)
        {
            Context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            var entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = IncludeProperties(includeProperties, query);

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync(cancellationToken);
            }
            else
            {
                return await query.ToListAsync(cancellationToken);
            }
        }

        private static IQueryable<TEntity> IncludeProperties(string includeProperties, IQueryable<TEntity> query)
        {
            foreach (var includeProperty in includeProperties.Split
                            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public virtual async Task<TEntity> GetByIDAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        public virtual async Task<TEntity> GetByIDAsync(object id, string includeProperties = "", CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _dbSet;

            query = query.Where(entity => entity.Id == (int)id);

            query = IncludeProperties(includeProperties, query);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public virtual async Task DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            await DeleteAsync(entityToDelete, cancellationToken);
        }
        public async Task DeleteAsync(TEntity entityToDelete, CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                if (Context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    _dbSet.Attach(entityToDelete);
                }

                _dbSet.Remove(entityToDelete);
            }, cancellationToken);
        }

        public async Task UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                _dbSet.Attach(entityToUpdate);
                Context.Entry(entityToUpdate).State = EntityState.Modified;
            }, cancellationToken);
        }
    }
}
