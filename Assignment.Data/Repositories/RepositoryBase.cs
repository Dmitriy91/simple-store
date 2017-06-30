using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Assignment.Data.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        #region Fields
        protected DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;
        #endregion

        public RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public virtual void Add(params TEntity[] entities)
        {
            foreach (TEntity entity in entities)
                _dbSet.Add(entity);
        }

        public virtual void Update(params TEntity[] entities)
        {
            foreach (TEntity entity in entities)
            {
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void Delete(params TEntity[] entities)
        {
            foreach (TEntity entity in entities)
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
            }
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
                return;

            _dbSet.RemoveRange(_dbSet.Where(condition));
        }

        public virtual TEntity GetById(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public virtual TEntity GetSingle(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
                return null;

            return _dbSet.FirstOrDefault(condition);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public virtual IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
                return null;

            return _dbSet.Where(condition);
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
                return false;

            return _dbSet.Any(condition);
        }

        public virtual IQueryable<TEntity> ExecuteQuery(string sql, params object[] parameters)
        {
            return _dbSet.SqlQuery(sql, parameters)?.AsQueryable();
        }

        public virtual IQueryable<TResult> ExecuteQuery<TResult>(string sql, params object[] parameters)
        {
            return _dbContext.Database.SqlQuery<TResult>(sql, parameters)?.AsQueryable();
        }
    }
}
