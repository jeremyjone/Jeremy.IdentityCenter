using System;
using System.Linq;
using System.Linq.Expressions;

namespace Jeremy.IdentityCenter.Database.Common.Extensions
{
    public static class QueryableExtension
    {
        /// <summary>
        /// 按页码查询指定内容。<br />
        /// 按条件排序，默认逆向排序。然后根据页码和每页大小获取指定内容。<br />
        /// 【注意】 在使用该查询之前需要先筛选实体内容，否则可能导致获取内容不正确。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="query">实体序列</param>
        /// <param name="orderBy">排序条件表达式</param>
        /// <param name="page">要获取的页码</param>
        /// <param name="pageSize">每页长度</param>
        /// <param name="isDescending">排序方向。默认逆序</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static IQueryable<T> PageBy<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> orderBy,
            int page, int pageSize, bool isDescending = true)
        {
            if (query == null) throw new ArgumentNullException(nameof(query), "Query contents can not be null.");

            if (page <= 0) page = 1;
            if (orderBy != null) query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 返回数据中指定长度的序列。如果长度为0，返回全部内容。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static IQueryable<T> TakeIf<T>(this IQueryable<T> query, int count)
        {
            if (query == null) throw new ArgumentNullException(nameof(query), "Query contents can not be null.");
            return count > 0 ? query.Take(count) : query;
        }

        /// <summary>
        /// 按条件是否需要筛选内容。当条件不满足时，不筛选。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="condition">满足条件。只有当满足该条件时才会筛选</param>
        /// <param name="expression">筛选表达式</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
            Expression<Func<T, bool>> expression)
        {
            if (query == null) throw new ArgumentNullException(nameof(query), "Query contents can not be null.");
            return condition ? query.Where(expression) : query;
        }
    }
}
