using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories.Interfaces;
using Jeremy.IdentityCenter.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Repositories.Interfaces
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        #region 客户端

        /// <summary>
        /// 获取指定客户端
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<Client> GetAsync(int clientId);

        #endregion





        /// <summary>
        /// 获取指定客户端秘钥
        /// </summary>
        /// <param name="secretId"></param>
        /// <returns></returns>
        Task<ClientSecret> GetClientSecretAsync(int secretId);

        /// <summary>
        /// 添加一个客户端秘钥
        /// </summary>
        /// <param name="secretsClientId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddClientSecretAsync(int secretsClientId, ClientSecret entity);

        /// <summary>
        /// 删除一个客户端秘钥
        /// </summary>
        /// <param name="secret"></param>
        /// <returns></returns>
        Task<bool> DeleteClientSecretAsync(ClientSecret secret);

        /// <summary>
        /// 获取客户端秘钥的集合
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<ClientSecret>> GetClientSecretsAsync(int clientId, int page, int pageSize);

        /// <summary>
        /// 获取指定客户端属性
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        Task<ClientProperty> GetClientPropertyAsync(int propertyId);

        /// <summary>
        /// 添加一个客户端属性
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        Task<bool> AddClientPropertyAsync(int clientId, ClientProperty property);

        /// <summary>
        /// 删除一个客户端属性
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        Task<bool> DeleteClientPropertyAsync(ClientProperty property);

        /// <summary>
        /// 获取客户端属性的集合
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<ClientProperty>> GetClientPropertiesAsync(int clientId, int page, int pageSize);

        /// <summary>
        /// 获取指定客户端声明
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        Task<ClientClaim> GetClientClaimAsync(int claimId);

        /// <summary>
        /// 添加一个客户端声明
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        Task<bool> AddClientClaimAsync(int clientId, ClientClaim claim);

        /// <summary>
        /// 删除一个客户端声明
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        Task<bool> DeleteClientClaimAsync(ClientClaim claim);

        /// <summary>
        /// 获取客户端声明的集合
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<ClientClaim>> GetClientClaimsAsync(int clientId, int page, int pageSize = 10);



        /// <summary>
        /// 获取客户端范围
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<string>> GetScopesAsync(string scope, int count = 0);

        /// <summary>
        /// 获取授权类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<string> GetGrantTypes(string type, int count = 0);

        /// <summary>
        /// 获取签入算法
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<string> GetSigningAlgorithms(string algorithm, int count = 0);

        /// <summary>
        /// 获取 access_token 类型
        /// </summary>
        /// <returns></returns>
        List<SelectItem> GetAccessTokenTypes();

        /// <summary>
        /// 获取 token usage 类型
        /// </summary>
        /// <returns></returns>
        List<SelectItem> GetTokenUsages();

        /// <summary>
        /// 获取 token 有效期的方式类型
        /// </summary>
        /// <returns></returns>
        List<SelectItem> GetTokenExpirations();

        /// <summary>
        /// 获取 hash 的类型
        /// </summary>
        /// <returns></returns>
        List<SelectItem> GetHashTypes();

        /// <summary>
        /// 获取协议类型
        /// </summary>
        /// <returns></returns>
        List<SelectItem> GetProtocolTypes();

        /// <summary>
        /// 获取加密类型
        /// </summary>
        /// <returns></returns>
        List<SelectItem> GetSecretTypes();

        List<string> GetStandardClaims(string claim, in int count);

        /// <summary>
        /// 判断客户端是否存在
        /// </summary>
        /// <param name="client"></param>
        /// <param name="isCloned"></param>
        /// <returns></returns>
        Task<bool> ExistClientAsync(Client client);
    }
}
