using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;

namespace RepositorySample.Persistence.Repository
{
    public abstract class GenericRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        [ImportingConstructor]
        public GenericRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IQueryable<TEntity> QueryContext { get { return UnitOfWorkContext.Set<TEntity, TKey>(); } }

        public IUnitOfWork UnitOfWork { get; }

        protected UnitOfWorkContext UnitOfWorkContext
        {
            get
            {
                if (UnitOfWork is UnitOfWorkContext)
                    return UnitOfWork as UnitOfWorkContext;
                throw new Exception("数据库上下文对象类型不正确");
            }
        }

        public virtual void Add(TEntity entity)
        {
            UnitOfWorkContext.RegisterNew<TEntity, TKey>(entity);
            UnitOfWorkContext.Commit();
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            UnitOfWorkContext.RegisterNew<TEntity, TKey>(entities);
            UnitOfWorkContext.Commit();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return UnitOfWorkContext.DataContext.Set<TEntity>().Count(predicate);
        }

        public void Delete(TKey id)
        {
            TEntity entity = UnitOfWorkContext.DataContext.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public void Delete(TEntity entity)
        {
            UnitOfWorkContext.RegisterDelete<TEntity, TKey>(entity);
            UnitOfWorkContext.Commit();
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            UnitOfWorkContext.RegisterDelete<TEntity, TKey>(entities);
            UnitOfWorkContext.Commit();
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            List<TEntity> entities = UnitOfWorkContext.DataContext.Set<TEntity>().Where(predicate).ToList();
            Delete(entities);
        }

        public TEntity Find(TKey id)
        {
            return UnitOfWorkContext.Set<TEntity, TKey>().Find(id);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return UnitOfWorkContext.Set<TEntity, TKey>().FirstOrDefault(predicate);
        }

        public bool IsExist(Expression<Func<TEntity, bool>> predicate)
        {
            return UnitOfWorkContext.Set<TEntity, TKey>().Any(predicate);
        }

        public IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> predicute)
        {
            return this.UnitOfWorkContext.Set<TEntity, TKey>().Where(predicute).ToList();
        }

        public IEnumerable<TEntity> Query(int pageIndex = 1, int pageSize = 10,
            Expression<Func<TEntity, bool>> predicute = null, Expression<Func<TEntity, object>> order = null)
        {
            if (pageIndex <= 0) throw new ArgumentException("页码不能小于1");
            if (pageSize <= 0) throw new ArgumentException("页容量不能小于0");
            if (order == null) order = o => o.Id;
            return UnitOfWorkContext.Set<TEntity, TKey>().Where(predicute).OrderBy(order)
                .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public void Update(TEntity entity)
        {
            UnitOfWorkContext.RegisterModified<TEntity, TKey>(entity);
            UnitOfWorkContext.Commit();
        }

        public void Update(Expression<Func<TEntity, object>> propertyExpression, TEntity entity)
        {
            UnitOfWorkContext.RegisterModified<TEntity, TKey>(propertyExpression, entity);
            var dbSet = UnitOfWorkContext.DataContext.Set<TEntity>();
            dbSet.Local.Clear();
            var entry = UnitOfWorkContext.DataContext.Entry(entity);
            UnitOfWorkContext.Commit();
        }
    }
}