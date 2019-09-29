using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data;

namespace RepositorySample.Persistence.Repository
{
    public interface IUnitOfWork
    {
        bool IsCommitted { get; }

        int Commit();

        void Rollback();
    }

    public interface IUnitOfWorkContext : IUnitOfWork, IDisposable
    {
        void RegisterNew<TEntity, TKey>(TEntity entity) where TEntity : class, IAggregateRoot<TKey> where TKey : IEquatable<TKey>;
        void RegisterNew<TEntity, TKey>(IEnumerable<TEntity> entities) where TEntity : class, IAggregateRoot<TKey> where TKey : IEquatable<TKey>;
        void RegisterModified<TEntity, TKey>(TEntity entity) where TEntity : class, IAggregateRoot<TKey> where TKey : IEquatable<TKey>;
        void RegisterModified<TEntity, TKey>(Expression<Func<TEntity, object>> propertyExpression, TEntity entity) where TEntity : class, IAggregateRoot<TKey> where TKey : IEquatable<TKey>;
        void RegisterDelete<TEntity, TKey>(TEntity entity) where TEntity : class, IAggregateRoot<TKey> where TKey : IEquatable<TKey>;
        void RegisterDelete<TEntity, TKey>(IEnumerable<TEntity> entities) where TEntity : class, IAggregateRoot<TKey> where TKey : IEquatable<TKey>;
    }

    [Export(typeof(IUnitOfWork))]
    public class UnitOfWorkContext : IUnitOfWorkContext
    {
        [ImportingConstructor]
        public UnitOfWorkContext(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public DataContext DataContext { get; }

        public bool IsCommitted { get; private set; }

        public int Commit()
        {
            if (IsCommitted) return 0;
            try
            {
                int result = DataContext.SaveChanges();
                IsCommitted = true;
                return result;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException is SqlException)
                {
                    //SqlException sqlEx = e.InnerException.InnerException as SqlException;
                    //string msg = DataHelper.GetSqlExceptionMessage(sqlEx.Number);
                    //throw PublicHelper.ThrowDataAccessException("提交数据更新时发生异常：" + msg, sqlEx);


                    //处理异常为自定义
                    SqlException sqlException = ex.InnerException.InnerException as SqlException;
                    string msg = "";
                    throw new Exception("" + msg, sqlException);
                }

                throw;
            }
        }

        public void Rollback()
        {
            IsCommitted = false;
        }

        public void Dispose()
        {
            if (!IsCommitted)
                Commit();
            DataContext.Dispose();
        }

        public DbSet<TEntity> Set<TEntity, TKey>() where TEntity : class, IAggregateRoot<TKey> where TKey : IEquatable<TKey>
        {
            return DataContext.Set<TEntity>();
        }

        public void RegisterNew<TEntity, TKey>(TEntity entity) where TEntity : class, IAggregateRoot<TKey> where TKey : IEquatable<TKey>
        {
            EntityState state = DataContext.Entry(entity).State;
            if (state == EntityState.Deleted)
            {
                DataContext.Entry(entity).State = EntityState.Added;
            }
            IsCommitted = false;
        }

        public void RegisterNew<TEntity, TKey>(IEnumerable<TEntity> entities) where TEntity : class, IAggregateRoot<TKey> where TKey : IEquatable<TKey>
        {
            try
            {
                DataContext.Configuration.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entities)
                {
                    RegisterNew<TEntity, TKey>(entity);
                }
            }
            finally
            {
                DataContext.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        public void RegisterModified<TEntity, TKey>(TEntity entity) where TEntity : class, IAggregateRoot<TKey> where TKey : IEquatable<TKey>
        {
            var entitySet = DataContext.Set<TEntity>();
            var entry = DataContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                entitySet.Attach(entity);
                entry.State = EntityState.Modified;
            }

            IsCommitted = false;
        }

        public void RegisterModified<TEntity, TKey>(Expression<Func<TEntity, object>> propertyExpression, TEntity entity)
            where TEntity : class, IAggregateRoot<TKey>
            where TKey : IEquatable<TKey>
        {
            if (propertyExpression == null) throw new ArgumentNullException(nameof(propertyExpression));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var entitySet = DataContext.Set<TEntity>();
            var entry = DataContext.Entry(entity);
            ReadOnlyCollection<MemberInfo> memberInfos = ((dynamic)propertyExpression.Body).Members;

            try
            {
                entry.State = EntityState.Unchanged;

                foreach (var memberInfo in memberInfos)
                {
                    entry.Property(memberInfo.Name).IsModified = true;
                }
            }
            catch (InvalidOperationException)
            {
                TEntity originalEntity = entitySet.Local.SingleOrDefault(m => Equals(m.Id, entity.Id));
                ObjectStateEntry objectStateEntry = DataContext.ObjectContext.ObjectStateManager.GetObjectStateEntry(originalEntity);
                objectStateEntry.ApplyCurrentValues(entity);
                objectStateEntry.ChangeState(EntityState.Unchanged);
                foreach (var memberInfo in memberInfos)
                {
                    objectStateEntry.SetModifiedProperty(memberInfo.Name);
                }
            }
            IsCommitted = false;
        }

        public void RegisterDelete<TEntity, TKey>(TEntity entity)
            where TEntity : class, IAggregateRoot<TKey>
            where TKey : IEquatable<TKey>
        {
            DataContext.Entry(entity).State = EntityState.Deleted;
            IsCommitted = false;
        }

        public void RegisterDelete<TEntity, TKey>(IEnumerable<TEntity> entities)
            where TEntity : class, IAggregateRoot<TKey>
            where TKey : IEquatable<TKey>
        {
            try
            {
                DataContext.Configuration.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entities)
                {
                    RegisterDelete<TEntity, TKey>(entity);
                }
            }
            finally
            {
                DataContext.Configuration.AutoDetectChangesEnabled = true;
            }
        }
    }
}
