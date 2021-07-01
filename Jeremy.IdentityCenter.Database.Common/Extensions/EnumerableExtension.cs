using Jeremy.IdentityCenter.Database.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jeremy.IdentityCenter.Database.Common.Extensions
{
    public static class EnumerableExtension
    {
        /// <summary>
        /// 返回数据中指定长度的序列。如果长度为0，返回全部内容。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static IEnumerable<T> TakeIf<T>(this IEnumerable<T> query, int count)
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
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query, bool condition,
            Func<T, bool> expression)
        {
            if (query == null) throw new ArgumentNullException(nameof(query), "Query contents can not be null.");
            return condition ? query.Where(expression) : query;
        }

        /// <summary>
        /// 将一个序列转换为页列表对象。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="list">当前序列段</param>
        /// <param name="totalCount">序列内容的总长度</param>
        /// <param name="pageSize">单页长度</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static PageList<TEntity> ToPageList<TEntity>(this IEnumerable<TEntity> list, int totalCount, int pageSize) where TEntity : class
        {
            if (list == null) throw new ArgumentNullException(nameof(list), "Contents can not be null.");
            return new PageList<TEntity>(list, totalCount, pageSize);
        }
    }
}
