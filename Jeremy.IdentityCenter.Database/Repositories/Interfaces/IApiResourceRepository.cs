using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Repositories.Interfaces
{
    public interface IApiResourceRepository : IBaseRepository<ApiResource>
    {
        /// <summary>
        /// 获取指定 Id 的 Api 资源内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResource> GetAsync(int id);


        /// <summary>
        /// 获取指定 Api 资源属性
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        Task<ApiResourceProperty> GetApiResourcePropertyAsync(int propertyId);

        /// <summary>
        /// 获取 Api 资源属性的集合
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<ApiResourceProperty>> GetApiResourcePropertiesAsync(int resourceId, int page = 1, int pageSize = 10);

        /// <summary>
        /// 添加一个 Api 资源属性
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddApiResourcePropertyAsync(int resourceId, ApiResourceProperty entity);

        /// <summary>
        /// 删除一个 Api 资源属性
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        Task<bool> DeleteApiResourcePropertyAsync(ApiResourceProperty property);

        /// <summary>
        /// 获取 Api 资源的秘钥
        /// </summary>
        /// <param name="secretId"></param>
        /// <returns></returns>
        Task<ApiResourceSecret> GetApiResourceSecretAsync(int secretId);

        /// <summary>
        /// 获取 Api 资源的秘钥集合
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<ApiResourceSecret>> GetApiResourceSecretsAsync(int resourceId, int page = 1, int pageSize = 10);

        /// <summary>
        /// 添加一个 Api 资源的秘钥
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddApiResourceSecretAsync(int resourceId, ApiResourceSecret entity);

        /// <summary>
        /// 删除一个 Api 资源的秘钥
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        Task<bool> DeleteApiResourceSecretAsync(ApiResourceSecret secret);

        /// <summary>
        /// 判断 Api 资源是否存在
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        Task<bool> ExistApiResourceAsync(ApiResource resource);

        /// <summary>
        /// 判断 Api 资源属性是否存在
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        Task<bool> ExistApiResourcePropertyAsync(ApiResourceProperty property);
    }
}
