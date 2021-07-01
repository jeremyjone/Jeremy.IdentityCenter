using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Repositories.Interfaces
{
    public interface IIdentityResourceRepository : IBaseRepository<IdentityResource>
    {
        /// <summary>
        /// 获取指定 Id 的身份资源内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IdentityResource> GetAsync(int id);


        /// <summary>
        /// 获取指定身份资源属性
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        Task<IdentityResourceProperty> GetIdentityResourcePropertyAsync(int propertyId);

        /// <summary>
        /// 获取身份资源属性的集合
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<IdentityResourceProperty>> GetIdentityResourcePropertiesAsync(int resourceId, int page, int pageSize);

        /// <summary>
        /// 添加一个身份资源属性
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddIdentityResourcePropertyAsync(int resourceId, IdentityResourceProperty entity);

        /// <summary>
        /// 删除一个身份资源属性
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        Task<bool> DeleteIdentityResourcePropertyAsync(IdentityResourceProperty property);



        /// <summary>
        /// 判断身份资源是否存在
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        Task<bool> ExistIdentityResourceAsync(IdentityResource resource);

        /// <summary>
        /// 判断身份资源属性是否存在
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        Task<bool> ExistIdentityResourcePropertyAsync(IdentityResourceProperty property);

    }
}
