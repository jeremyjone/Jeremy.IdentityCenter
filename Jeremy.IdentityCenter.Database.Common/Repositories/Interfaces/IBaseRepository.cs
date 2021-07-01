using Jeremy.IdentityCenter.Database.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Common.Repositories.Interfaces
{
    /// <summary>
    /// 基础增删改查接口
    /// </summary>
    public interface IBaseRepository<TEntity> : IRepository where TEntity : class
    {
        #region 查

        /// <summary>
        /// 根据条件获取内容
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        TEntity Get(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 根据条件异步获取内容
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);


        /// <summary>
        /// 获取全部内容<br />
        /// ** <b>慎用</b> ** 数量过多时，可能会卡。
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        List<TEntity> GetRange(Expression<Func<TEntity, bool>> expression = null);

        /// <summary>
        /// 异步获取全部内容
        /// ** <b>慎用</b> ** 数量过多时，可能会卡。
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetRangeAsync(Expression<Func<TEntity, bool>> expression = null);

        /// <summary>
        /// 获取指定条件下当前页的内容
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="expression"></param>
        /// <param name="orderBy"></param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">页内数量，默认10</param>
        /// <param name="isDescending"></param>
        /// <returns></returns>
        PageList<TEntity> GetRange<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, int page = 1, int pageSize = 10, bool isDescending = false);

        /// <summary>
        /// 获取指定条件下当前页的内容
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="orderBy"></param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">页内数量，默认10</param>
        /// <param name="isDescending"></param>
        Task<PageList<TEntity>> GetRangeAsync<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, int page = 1, int pageSize = 10, bool isDescending = false);

        /// <summary>
        /// 获取指定条件下当前页的内容
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="expression"></param>
        /// <param name="orderBy"></param>
        /// <param name="param">参数对象</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">页内数量，默认10</param>
        /// <param name="isDescending"></param>
        /// <returns></returns>
        Task<PageList<TEntity>> GetRangeAsync<TParam, TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, TParam param = null, int page = 1, int pageSize = 10, bool isDescending = false) where TParam : class;

        /// <summary>
        /// 获取指定条件下当前页的内容。
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<TEntity>> GetRangeAsync(Expression<Func<TEntity, bool>> expression, int page, int pageSize = 10);

        /// <summary>
        /// 获取指定条件的附属内容
        /// </summary>
        /// <typeparam name="TAccessory"></typeparam>
        /// <param name="expression"></param>
        /// <param name="includeExpression"></param>
        /// <returns></returns>
        Task<TAccessory> GetAccessoryAsync<TAccessory>(Expression<Func<TAccessory, bool>> expression, Expression<Func<TAccessory, TEntity>> includeExpression = null) where TAccessory : class, new();

        /// <summary>
        /// 获取指定条件的附属内容
        /// </summary>
        /// <typeparam name="TAccessory"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="expression"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<TAccessory>> GetAccessoryRangeAsync<TAccessory, TKey>(Expression<Func<TAccessory, bool>> expression, Expression<Func<TAccessory, TKey>> orderBy, int page, int pageSize) where TAccessory : class, new();

        #endregion



        #region 增

        /// <summary>
        /// 添加一个内容
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add(TEntity entity);

        /// <summary>
        /// 异步添加一个内容
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddAsync(TEntity entity);

        /// <summary>
        /// 添加多个内容
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 异步添加多个内容
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 异步添加一个附属实体内容
        /// </summary>
        /// <typeparam name="TAccessory"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddAccessoryAsync<TAccessory>(TAccessory entity) where TAccessory : class, new();

        #endregion



        #region 改

        /// <summary>
        /// 更新一个内容
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(TEntity entity);

        /// <summary>
        /// 更新一个内容
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity);

        /// <summary>
        /// 更新多个内容
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新多个内容
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities);

        #endregion



        #region 删

        /// <summary>
        /// 删除指定内容
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete(TEntity entity);

        /// <summary>
        /// 删除指定内容
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Delete(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 删除多个内容
        /// </summary>
        /// <param name="entities"></param>
        bool DeleteRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 删除多个内容
        /// </summary>
        /// <param name="expression"></param>
        bool DeleteRange(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 异步删除多个内容
        /// </summary>
        /// <param name="expression"></param>
        Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 删除一个附属实体内容
        /// </summary>
        /// <typeparam name="TAccessory"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeleteAccessoryAsync<TAccessory>(TAccessory entity) where TAccessory : class, new();

        /// <summary>
        /// 删除一个附属实体内容
        /// </summary>
        /// <typeparam name="TAccessory"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<bool> DeleteAccessoryAsync<TAccessory>(Expression<Func<TAccessory, bool>> expression) where TAccessory : class, new();

        #endregion


        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        bool Save();

        /// <summary>
        /// 异步保存
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();
    }
}
