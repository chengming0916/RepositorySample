using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RepositorySample.Persistence
{
    [Export(typeof(DataContext)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class DataContext : DbContext
    {
        [ImportingConstructor]
        public DataContext() : base("Name=MyContext")
        {
#if RELEASE
            //启用数据迁移
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Migrations.Configuration>());
#endif

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //移除一对多的级联删除约定，想要级联删除可以在 EntityTypeConfiguration<TEntity>的实现类中进行控制
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //多对多启用级联删除约定，不想级联删除可以在删除前判断关联的数据进行拦截
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            //移除复数表名的契约
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            //自动注册Model配置
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType
                && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic instance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(instance);
            }
        }

        public ObjectContext ObjectContext
        {
            get
            {
                return ((IObjectContextAdapter)this).ObjectContext;
            }
        }

        public IQueryable<T> NoTrackable<T>() where T : class, IAggregateRoot
        {
            ObjectSet<T> objectSet = ObjectContext.CreateObjectSet<T>();
            objectSet.MergeOption = MergeOption.NoTracking;
            return objectSet.OfType<T>();
        }

        public IQueryable<T> Trackable<T>() where T : class, IAggregateRoot
        {
            return ObjectContext.CreateObjectSet<T>();
        }
    }
}
