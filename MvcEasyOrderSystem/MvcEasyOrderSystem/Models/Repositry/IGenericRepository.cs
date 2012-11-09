using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MvcEasyOrderSystem.Models.Repositry
{

    public interface IGenericRepository<TEntity> : IDisposable

    where TEntity : class
    {

        void Insert(TEntity instance);



        void Update(TEntity instance);



        void Delete(TEntity instance);



        IEnumerable<TEntity> GetWithFilterAndOrder(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "");



        TEntity GetSingleEntity(Expression<Func<TEntity, bool>> predicate);


        void SaveChanges();



    }

}

