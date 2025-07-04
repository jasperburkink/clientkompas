﻿using System.Linq.Expressions;
using Application.Common.Interfaces.CVS;
using Domain.Common;
using Domain.CVS.Domain;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.CVS
{
    public class GenericRepository<TEntity>(CVSDbContext context) : IRepository<TEntity> where TEntity : BaseEntity
    {
        public CVSDbContext Context { get; } = context;

        internal DbSet<TEntity> _dbSet = context.Set<TEntity>();

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
                ([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return [.. orderBy(query)];
            }
            else
            {
                return [.. query];
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
                            ([','], StringSplitOptions.RemoveEmptyEntries))
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

        // TODO: This doesn't belong here. Refactor!
        public async Task<Organization> GetByKVKNumberAsync(string kvkNumber, CancellationToken cancellationToken)
        {
            return await Context.Organizations.FirstOrDefaultAsync(o => o.KVKNumber == kvkNumber, cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Context.Set<TEntity>().AnyAsync(predicate, cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> FullTextSearch(string searchTerm, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] properties)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Context.Set<TEntity>();
            }

            var tableName = ContextExtensions.GetTableName(Context, typeof(TEntity));

            var searchTerms = searchTerm.Split([' '], StringSplitOptions.RemoveEmptyEntries);

            var sqlParameters = new List<object>();
            var whereClauses = new List<string>();

            var parameterIndex = 0;

            foreach (var prop in properties)
            {
                var propertyName = PropertyInfoHelper.GetPropertyInfo(prop).Name;
                foreach (var term in searchTerms)
                {
                    var paramName = $"@p{parameterIndex++}";
                    whereClauses.Add($"{propertyName} LIKE {paramName}");
                    sqlParameters.Add($"%{term}%");
                }
            }

            var combinedWhereClause = string.Join(" OR ", whereClauses);

            var query = $"SELECT * FROM {tableName} WHERE {combinedWhereClause}";

            return await _dbSet.FromSqlRaw(query, [.. sqlParameters]).ToListAsync(cancellationToken);
        }


        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }
    }
}
