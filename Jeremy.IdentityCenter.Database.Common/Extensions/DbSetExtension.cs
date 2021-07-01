using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Jeremy.IdentityCenter.Database.Common.Extensions
{
    public static class DbSetExtension
    {
        /// <summary>
        /// 【数据库操作扩展方法】删除符合条件的实体，不含保存。<br />
        /// 开始跟踪符合条件的实体的 <see cref="EntityState.Deleted" /> 状态，
        /// 当调用 <see cref="DbContext.SaveChanges()" /> 的时候，它们将从数据库中被删除。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="db"></param>
        /// <param name="expression"></param>
        public static void RemoveRange<TEntity>(this DbSet<TEntity> db, Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            var list = db.Where(expression);
            db.RemoveRange(list);
        }

        /// <summary>
        /// 当条件不为 null 时，按条件引用实体内容
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="source"></param>
        /// <param name="navigationPropertyPath"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static IQueryable<TEntity> IncludeIf<TEntity, TProperty>(
            this DbSet<TEntity> source,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TEntity : class
        {
            if (source == null) throw new ArgumentNullException(nameof(source), "Source can not be null.");
            return navigationPropertyPath == null ? source.AsQueryable() : source.Include(navigationPropertyPath);
        }
    }
}
