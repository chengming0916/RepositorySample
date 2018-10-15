using Infrastructure.Data;
using RepositorySample.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace RepositorySample.Persistence.Repository
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>

    {
        /// <summary>
        /// 实体查询数据集
        /// </summary>
        IQueryable<TEntity> QueryContext { get; }

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Delete(TKey id);

        void Delete(TEntity entity);

        void Delete(IEnumerable<TEntity> entities);

        void Delete(Expression<Func<TEntity, bool>> predicate);

        void Update(TEntity entity);

        void Update(Expression<Func<TEntity, object>> propertyExpression, TEntity entity);

        TEntity Find(TKey id);

        IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> predicute);

        IEnumerable<TEntity> Query(int pageIndex = 1, int pageSize = 10, Expression<Func<TEntity, bool>> expression = null, Expression<Func<TEntity, object>> order = null);

        bool IsExist(Expression<Func<TEntity, bool>> expression);

        int Count(Expression<Func<TEntity, bool>> predicute = null);
    }
}