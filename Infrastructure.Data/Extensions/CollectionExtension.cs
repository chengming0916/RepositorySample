using Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Extensions
{
    public static class CollectionExtension
    {
        /// <summary>
        /// 从指定IQueryable<TEntity>集合 中查询指定分页条件的子数据集
        /// </summary>
        /// <typeparam name="TEntity">动态实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="source">要查询的数据集</param>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="total">输出符合条件的总记录数</param>
        /// <param name="sortConditions">排序条件集合</param>
        /// <returns></returns>
        public static IQueryable<TEntity> Where<TEntity, TKey>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> predicate, out int total, int pageIndex = 1, int pageSize = 10,
            PropertySortCondition[] sortConditions = null)
            where TEntity : IAggregateRoot<TKey>
            where TKey : IEquatable<TKey>
        {

            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;


            total = source.Count(predicate);
            if (sortConditions == null || sortConditions.Length == 0)
            {
                source = source.OrderBy(m => m.Id);
            }
            else
            {
                int count = 0;
                IOrderedQueryable<TEntity> orderSource = null;
                foreach (PropertySortCondition sortCondition in sortConditions)
                {
                    orderSource = count == 0
                        ? source.OrderBy(sortCondition.PropertyName, sortCondition.ListSortDirection)
                        : orderSource.ThenBy(sortCondition.PropertyName, sortCondition.ListSortDirection);
                    count++;
                }
                source = orderSource;
            }
            return source != null
                ? source.Where(predicate).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                : Enumerable.Empty<TEntity>().AsQueryable();
        }

    }
}
