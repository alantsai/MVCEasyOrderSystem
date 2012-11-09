using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MvcEasyOrderSystem.Models.Repositry
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>

        where TEntity : class
    {
        internal DbContext _dbContext { get; set; }
        internal DbSet<TEntity> _dbSet { get; set; }

        public GenericRepository(DbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<TEntity>();
        }

        public GenericRepository()
            : this(new EOSystemContex())
        {
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            Delete(GetSingleEntity(predicate));
        }

        public void Delete(object id)
        {
            Delete(GetSingleEntity(id));
        }

        public IEnumerable<TEntity> GetWithFilterAndOrder
            (System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, 
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

        public TEntity GetSingleEntity
            (System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public TEntity GetSingleEntity(object id)
        {
            return _dbSet.Find(id);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {

            this.Dispose(true);

            GC.SuppressFinalize(this);

        }



        protected virtual void Dispose(bool disposing)
        {

            if (disposing)
            {

                if (this._dbContext != null)
                {

                    this._dbContext.Dispose();

                    this._dbContext = null;

                }

            }

        }





    }
}