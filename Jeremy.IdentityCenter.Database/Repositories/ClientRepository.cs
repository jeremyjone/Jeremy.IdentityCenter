using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Jeremy.IdentityCenter.Database.Common.Extensions;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories;
using Jeremy.IdentityCenter.Database.Constants;
using Jeremy.IdentityCenter.Database.DbContexts.Interfaces;
using Jeremy.IdentityCenter.Database.Enums;
using Jeremy.IdentityCenter.Database.Helper;
using Jeremy.IdentityCenter.Database.Models;
using Jeremy.IdentityCenter.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Client = IdentityServer4.EntityFramework.Entities.Client;
using ClientClaim = IdentityServer4.EntityFramework.Entities.ClientClaim;

namespace Jeremy.IdentityCenter.Database.Repositories
{
    public class ClientRepository<TContext> : BaseRepository<Client, TContext, IClientRepository>, IClientRepository
        where TContext : DbContext, IIdcConfigurationDbContext
    {

        public ClientRepository(TContext db, ILogger<IClientRepository> logger) : base(db, logger)
        {
        }

        public Task<Client> GetAsync(int clientId)
        {
            return Db.Clients
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.Properties)
                .Where(x => x.Id == clientId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public override async Task<PageList<Client>> GetRangeAsync(Expression<Func<Client, bool>> expression, int page,
            int pageSize = 10)
        {
            return await GetRangeAsync(expression, x => x.Id, page, pageSize, true);
        }

        public override async Task<bool> UpdateAsync(Client client)
        {
            // 删除旧的关联内容
            RemoveClientRelations(client);

            return await base.UpdateAsync(client);
        }



        #region 客户端秘钥

        public async Task<ClientSecret> GetClientSecretAsync(int secretId)
        {
            return await GetAccessoryAsync<ClientSecret>(x => x.Id == secretId, x => x.Client);
        }

        public async Task<PageList<ClientSecret>> GetClientSecretsAsync(int clientId, int page, int pageSize)
        {
            return await GetAccessoryRangeAsync<ClientSecret, int>(x => x.Client.Id == clientId, x => x.Id, page,
                pageSize);
        }

        public async Task<bool> AddClientSecretAsync(int clientId, ClientSecret entity)
        {
            entity.Client = await GetAsync(x => x.Id == clientId);
            return await AddAccessoryAsync(entity);
        }

        public async Task<bool> DeleteClientSecretAsync(ClientSecret secret)
        {
            return await DeleteAccessoryAsync<ClientSecret>(x => x.Id == secret.Id);
        }

        #endregion



        #region 客户端属性

        public async Task<ClientProperty> GetClientPropertyAsync(int propertyId)
        {
            return await GetAccessoryAsync<ClientProperty>(x => x.Id == propertyId, x => x.Client);
        }

        public async Task<PageList<ClientProperty>> GetClientPropertiesAsync(int clientId, int page, int pageSize)
        {
            return await GetAccessoryRangeAsync<ClientProperty, int>(x => x.Client.Id == clientId, x => x.Id, page,
                pageSize);
        }

        public async Task<bool> AddClientPropertyAsync(int clientId, ClientProperty entity)
        {
            entity.Client = await GetAsync(x => x.Id == clientId);
            return await AddAccessoryAsync(entity);
        }

        public async Task<bool> DeleteClientPropertyAsync(ClientProperty property)
        {
            return await DeleteAccessoryAsync<ClientProperty>(x => x.Id == property.Id);
        }

        #endregion


        #region 客户端声明

        public async Task<ClientClaim> GetClientClaimAsync(int claimId)
        {
            return await GetAccessoryAsync<ClientClaim>(x => x.Id == claimId, x => x.Client);
        }

        public async Task<PageList<ClientClaim>> GetClientClaimsAsync(int clientId, int page, int pageSize)
        {
            return await GetAccessoryRangeAsync<ClientClaim, int>(x => x.Client.Id == clientId, x => x.Id, page,
                pageSize);
        }

        public async Task<bool> AddClientClaimAsync(int clientId, ClientClaim entity)
        {
            entity.Client = await GetAsync(x => x.Id == clientId);
            return await AddAccessoryAsync(entity);
        }

        public async Task<bool> DeleteClientClaimAsync(ClientClaim claim)
        {
            return await DeleteAccessoryAsync<ClientClaim>(x => x.Id == claim.Id);
        }

        #endregion


        public async Task<List<string>> GetScopesAsync(string scope, int count = 0)
        {
            var identityResources = await Db.IdentityResources
                .WhereIf(!string.IsNullOrWhiteSpace(scope), x => x.Name.Contains(scope))
                .OrderByDescending(x => x.Id)
                .TakeIf(count)
                .Select(x => x.Name).ToListAsync();

            var apiScopes = await Db.ApiScopes
                .WhereIf(!string.IsNullOrWhiteSpace(scope), x => x.Name.Contains(scope))
                .OrderByDescending(x => x.Id)
                .TakeIf(count)
                .Select(x => x.Name).ToListAsync();

            var scopes = identityResources.Concat(apiScopes).OrderByDescending(x => x).TakeIf(count).ToList();

            return scopes;
        }

        public List<string> GetGrantTypes(string type, int count = 0)
        {
            return ClientConstants.GrantTypes
                .WhereIf(!string.IsNullOrWhiteSpace(type), x => x.Contains(type))
                .OrderByDescending(x => x)
                .TakeIf(count)
                .ToList();
        }

        public List<string> GetSigningAlgorithms(string algorithm, int count = 0)
        {
            return ClientConstants.SigningAlgorithms
                .WhereIf(!string.IsNullOrWhiteSpace(algorithm), x => x.Contains(algorithm))
                .OrderByDescending(x => x)
                .TakeIf(count)
                .ToList();
        }

        public virtual List<SelectItem> GetAccessTokenTypes()
        {
            return EnumHelper.EnumToList<AccessTokenType>();
        }

        public virtual List<SelectItem> GetTokenExpirations()
        {
            return EnumHelper.EnumToList<TokenExpiration>();
        }

        public virtual List<SelectItem> GetTokenUsages()
        {
            var tokenUsage = EnumHelper.EnumToList<TokenUsage>();
            return tokenUsage;
        }

        public virtual List<SelectItem> GetHashTypes()
        {
            var hashTypes = EnumHelper.EnumToList<HashType>();
            return hashTypes;
        }

        public virtual List<SelectItem> GetProtocolTypes()
        {
            return ClientConstants.ProtocolTypes;
        }

        public virtual List<SelectItem> GetSecretTypes()
        {
            return ClientConstants.SecretTypes.Select(x => new SelectItem(x, x)).ToList();
        }

        public List<string> GetStandardClaims(string claim, in int count)
        {
            return ClientConstants.StandardClaims.WhereIf(!string.IsNullOrWhiteSpace(claim), x => x.Contains(claim))
                .OrderByDescending(x => x).TakeIf(count).ToList();
        }

        public async Task<bool> ExistClientAsync(Client client)
        {
            return null != await Db.Clients
                .Where(x => x.ClientId == client.ClientId && (client.Id == 0 || x.Id != client.Id))
                .FirstOrDefaultAsync();
        }



        #region 工具函数

        /// <summary>
        /// 删除与客户端有关联的数据，更新时使用
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private void RemoveClientRelations(Client client)
        {
            Db.ClientScopes.RemoveRange(x => x.Client.Id == client.Id);
            Db.ClientGrantTypes.RemoveRange(x => x.Client.Id == client.Id);
            Db.ClientRedirectUris.RemoveRange(x => x.Client.Id == client.Id);
            Db.ClientCorsOrigins.RemoveRange(x => x.Client.Id == client.Id);
            Db.ClientIdPRestrictions.RemoveRange(x => x.Client.Id == client.Id);
            Db.ClientPostLogoutRedirectUris.RemoveRange(x => x.Client.Id == client.Id);
        }

        #endregion
    }
}