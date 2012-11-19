using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MvcEasyOrderSystem.Models.Repositry
{
    /// <summary>
    /// Repository Pattern用到的介面。可惜因為時間關係並未做Unit Of Work導致有些地方
    /// Repository太多不好管理。
    /// 本來在GetWithFilterAndOrder()想把一些常用的Linq包起來最後直接傳回對應的資料，但缺少Group的部份。
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
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

