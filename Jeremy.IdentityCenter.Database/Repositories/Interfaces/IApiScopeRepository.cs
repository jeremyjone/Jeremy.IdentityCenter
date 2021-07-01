using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Repositories.Interfaces
{
    public interface IApiScopeRepository : IBaseRepository<ApiScope>
    {
        /// <summary>
        /// 获取指定 Id 的 Api 作用域内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiScope> GetAsync(int id);


        /// <summary>
        /// 获取指定 Api 作用域属性
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        Task<ApiScopeProperty> GetApiScopePropertyAsync(int propertyId);

        /// <summary>
        /// 获取 Api 作用域属性的集合
        /// </summary>
        /// <param name="scopeId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<ApiScopeProperty>> GetApiScopePropertiesAsync(int scopeId, int page, int pageSize);

        /// <summary>
        /// 添加一个 Api 作用域属性
        /// </summary>
        /// <param name="scopeId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddApiScopePropertyAsync(int scopeId, ApiScopeProperty entity);

        /// <summary>
        /// 删除一个 Api 作用域属性
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        Task<bool> DeleteApiScopePropertyAsync(ApiScopeProperty property);


        /// <summary>
        /// 判断 Api 作用域是否存在
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        Task<bool> ExistApiScopeAsync(ApiScope scope);

        /// <summary>
        /// 判断 Api 作用域属性是否存在
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        Task<bool> ExistApiScopePropertyAsync(ApiScopeProperty property);
    }
}
