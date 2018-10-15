using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Extensions
{
    public static class CollectionExtension
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="collection">要处理的集合</param>
        /// <returns>为空返回True，不为空返回False</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return !collection.Any();
        }

        /// <summary>
        /// 集合转换为以指定字符分割的字符串
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="collection">待处理集合</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string ExpandToString<T>(this IEnumerable<T> collection, string separator)
        {
            List<T> source = collection as List<T> ?? collection.ToList();
            if (source.IsEmpty()) return string.Empty;
            string result = source.Cast<object>().Aggregate<object, string>(null, (current, o) => current + string.Format("{0}{1}", o, separator));
            return result.Substring(0, result.Length - separator.Length);
        }

        /// <summary>
        /// 根据第三方条件是否为真来决定是否执行指定条件的查询
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source">要查询的源</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="codition">第三方条件</param>
        /// <returns>查询的结果</returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool codition)
        {
            return codition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 根据指定条件返回集合中不重复的元素
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <typeparam name="TKey">动态筛选条件类型</typeparam>
        /// <param name="source">要操作的源</param>
        /// <param name="keySelector">重复数据筛选条件</param>
        /// <returns>不重复元素的集合</returns>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Select(group => group.First());
        }

        /// <summary>
        /// 根据第三方条件是否为真来决定是否执行指定条件的查询
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source">要查询的源</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="condition">第三方条件</param>
        /// <returns>查询的结果</returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            return predicate != null && condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 把IQueryable<T>集合按指定属性与排序方式进行排序
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source">要排序的数据集</param>
        /// <param name="propertyName">排序属性名</param>
        /// <param name="sortDirection">排序方向</param>
        /// <returns>排序后的数据集</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            return QueryableHelper<T>.OrderBy(source, propertyName, sortDirection);
        }

        /// <summary>
        /// 把IQueryable<T>集合按指定属性排序条件进行排序
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source">要排序的数据集</param>
        /// <param name="sortCondition">列表属性排序条件</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, PropertySortCondition sortCondition)
        {
            if (sortCondition == null || string.IsNullOrEmpty(sortCondition.PropertyName)) throw new ArgumentNullException(nameof(sortCondition));
            return source.OrderBy(sortCondition.PropertyName, sortCondition.ListSortDirection);
        }

        /// <summary>
        /// 把IOrderedQueryable[T]集合继续按指定属性排序方式进行排序
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source">要排序的数据集</param>
        /// <param name="propertyName">排序属性名</param>
        /// <param name="sortDirection">排序方向</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName,
            ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));
            return QueryableHelper<T>.ThenBy(source, propertyName, sortDirection);
        }

        /// <summary>
        /// 把IOrderedQueryable<T>集合继续指定属性排序方式进行排序
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source">要排序的数据集</param>
        /// <param name="sortCondition">列表属性排序条件</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, PropertySortCondition sortCondition)
        {
            if (sortCondition == null || string.IsNullOrEmpty(sortCondition.PropertyName)) throw new ArgumentNullException(nameof(sortCondition));
            return source.ThenBy(sortCondition.PropertyName, sortCondition.ListSortDirection);
        }

        private static class QueryableHelper<T>
        {
            private static readonly ConcurrentDictionary<string, LambdaExpression> Cache = new ConcurrentDictionary<string, LambdaExpression>();

            internal static IOrderedQueryable<T> OrderBy(IQueryable<T> source, string propertyName, ListSortDirection sortDirection)
            {
                dynamic keySelector = GetLambdaExpression(propertyName);
                return sortDirection == ListSortDirection.Ascending
                    ? Queryable.OrderBy(source, keySelector)
                    : Queryable.OrderByDescending(source, keySelector);
            }

            internal static IOrderedQueryable<T> ThenBy(IOrderedQueryable<T> source, string propertyName, ListSortDirection sortDirection)
            {
                dynamic keySelector = GetLambdaExpression(propertyName);
                return sortDirection == ListSortDirection.Ascending
                    ? Queryable.ThenBy(source, keySelector)
                    : Queryable.ThenByDescending(source, keySelector);
            }

            private static LambdaExpression GetLambdaExpression(string propertyName)
            {
                if (Cache.ContainsKey(propertyName))
                {
                    return Cache[propertyName];
                }
                ParameterExpression param = Expression.Parameter(typeof(T));
                MemberExpression body = Expression.Property(param, propertyName);
                LambdaExpression keySelector = Expression.Lambda(body, param);
                Cache[propertyName] = keySelector;
                return keySelector;
            }
        }
    }

    /// <summary>
    /// 属性排序条件信息类
    /// </summary>
    public class PropertySortCondition
    {
        /// <summary>
        ///     构造一个指定属性名称的升序排序的排序条件
        /// </summary>
        /// <param name="propertyName">排序属性名称</param>
        public PropertySortCondition(string propertyName)
            : this(propertyName, ListSortDirection.Ascending) { }

        /// <summary>
        ///     构造一个排序属性名称和排序方式的排序条件
        /// </summary>
        /// <param name="propertyName">排序属性名称</param>
        /// <param name="listSortDirection">排序方式</param>
        public PropertySortCondition(string propertyName, ListSortDirection listSortDirection)
        {
            PropertyName = propertyName;
            ListSortDirection = listSortDirection;
        }

        /// <summary>
        ///     获取或设置 排序属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        ///     获取或设置 排序方向
        /// </summary>
        public ListSortDirection ListSortDirection { get; set; }
    }
}