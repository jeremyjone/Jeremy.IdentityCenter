using IdentityServer4.Models;
using Jeremy.IdentityCenter.Business.Common.Models;
using Jeremy.IdentityCenter.Business.Mappers.Extension;
using Jeremy.IdentityCenter.Business.Models;
using Jeremy.IdentityCenter.Business.Models.Clients;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Jeremy.IdentityCenter.Database.Enums;
using Jeremy.IdentityCenter.Database.Repositories.Interfaces;
using Jeremy.Tools.Json;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Services
{
    public class ClientService : BaseService<IClientRepository, IClientService>, IClientService
    {
        private const string SharedSecret = "SharedSecret";

        public ClientService(IClientRepository repository, ILogger<IClientService> logger) : base(repository, logger)
        {
        }


        #region 客户端 Client

        public async Task<ClientViewModel> GetClientAsync(int clientId)
        {
            var client = await Repository.GetAsync(clientId);
            if (client == null) throw new NullResultException($"获取客户端 {clientId} 为空");

            return client.ToViewModel();
        }

        public async Task<ClientsViewModel> GetClientsAsync(string search, int page = 1, int pageSize = 10)
        {
            var clients =
                await Repository.GetRangeAsync(
                    x => string.IsNullOrWhiteSpace(search) || x.ClientName.Contains(search) ||
                         x.ClientId.Contains(search),
                    page, pageSize);
            return clients.ToViewModel();
        }

        public async Task<ClientViewModel> AddClientAsync(ClientViewModel client)
        {
            var entity = client.ToEntity();
            if (await Repository.ExistClientAsync(entity)) throw new NullResultException($"Can not create client [{client.ClientId}].");

            switch (client.ClientType)
            {
                case ClientType.Empty:
                    break;
                case ClientType.Web:
                    client.AllowedGrantTypes.AddRange(GrantTypes.Code);
                    client.RequirePkce = true;
                    client.RequireClientSecret = true;
                    break;
                case ClientType.Spa:
                    client.AllowedGrantTypes.AddRange(GrantTypes.Code);
                    client.RequirePkce = true;
                    client.RequireClientSecret = false;
                    break;
                case ClientType.Native:
                    client.AllowedGrantTypes.AddRange(GrantTypes.Code);
                    client.RequirePkce = true;
                    client.RequireClientSecret = false;
                    break;
                case ClientType.Machine:
                    client.AllowedGrantTypes.AddRange(GrantTypes.ClientCredentials);
                    break;
                case ClientType.Device:
                    client.AllowedGrantTypes.AddRange(GrantTypes.DeviceFlow);
                    client.RequireClientSecret = false;
                    client.AllowOfflineAccess = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var res = await Repository.AddAsync(entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<ClientViewModel> UpdateClientAsync(ClientViewModel client)
        {
            var entity = client.ToEntity();
            if (await Repository.ExistClientAsync(entity)) throw new NullResultException($"Can not update client [{client.ClientId}].");

            var res = await Repository.UpdateAsync(entity);
            return res ? entity.ToViewModel() : null;
        }

        public ClientViewModel RemoveClient(ClientViewModel client)
        {
            var res = Repository.Delete(client.ToEntity());
            return res ? client : null;
        }

        #endregion




        #region 客户端秘钥 ClientSecret

        public async Task<ClientSecretsViewModel> GetClientSecretAsync(int secretId)
        {
            var secret = await Repository.GetClientSecretAsync(secretId);
            if (secret == null) throw new NullResultException($"Invalid client secret id [{secretId}].");

            var client = await Repository.GetAsync(secret.Client.Id);
            if (client == null) throw new NullResultException($"Invalid client id [{secret.Client.Id}].");

            var model = secret.ToViewModel();
            model.ClientId = secret.Client.Id;
            model.ClientName = $"{client.ClientId}_{client.ClientName}";
            model.Value = null;
            return model;

        }

        public async Task<ClientSecretsViewModel> GetClientSecretsAsync(int clientId, int page, int pageSize = 10)
        {
            var client = await Repository.GetAsync(clientId);
            if (client == null) throw new NullResultException($"Invalid client id [{clientId}].");

            var secrets = await Repository.GetClientSecretsAsync(clientId, page, pageSize);
            var model = secrets.ToViewModel();
            model.ClientId = clientId;
            model.ClientName = CreateClientName(client.ClientId, client.ClientName);
            model.ClientSecrets.ForEach(x => x.Value = null);
            return model;
        }

        public async Task<ClientSecretsViewModel> AddClientSecretAsync(ClientSecretsViewModel secrets)
        {
            switch (secrets.Type)
            {
                // Hash 秘钥
                case SharedSecret:
                    {
                        if (secrets.HashTypeEnum == HashType.Sha256) secrets.Value = secrets.Value.Sha256();
                        if (secrets.HashTypeEnum == HashType.Sha512) secrets.Value = secrets.Value.Sha512();
                        break;
                    }
            }

            var entity = secrets.ToEntity();
            var res = await Repository.AddClientSecretAsync(secrets.ClientId, entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<ClientSecretsViewModel> RemoveClientSecretAsync(ClientSecretsViewModel secret)
        {
            var entity = secret.ToEntity();
            var res = await Repository.DeleteClientSecretAsync(entity);
            return res ? secret : null;
        }

        #endregion



        #region 客户端属性 ClientProperty

        public async Task<ClientPropertiesViewModel> GetClientPropertyAsync(int propertyId)
        {
            var property = await Repository.GetClientPropertyAsync(propertyId);
            if (property == null) throw new NullResultException($"Invalid client property id [{propertyId}].");

            var client = await Repository.GetAsync(property.Client.Id);
            if (client == null) throw new NullResultException($"Invalid client id [{property.Client.Id}].");

            var model = property.ToViewModel();
            model.ClientId = property.Client.Id;
            model.ClientName = CreateClientName(client.ClientId, client.ClientName);
            return model;
        }

        public async Task<ClientPropertiesViewModel> AddClientPropertyAsync(ClientPropertiesViewModel properties)
        {
            var entity = properties.ToEntity();
            var res = await Repository.AddClientPropertyAsync(properties.ClientId, entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<ClientPropertiesViewModel> RemoveClientPropertyAsync(ClientPropertiesViewModel property)
        {
            var entity = property.ToEntity();
            var res = await Repository.DeleteClientPropertyAsync(entity);
            return res ? property : null;
        }

        public async Task<ClientPropertiesViewModel> GetClientPropertiesAsync(int clientId, int page, int pageSize = 10)
        {
            var client = await Repository.GetAsync(clientId);
            if (client == null) throw new NullResultException($"Invalid client id [{clientId}].");

            var properties = await Repository.GetClientPropertiesAsync(clientId, page, pageSize);
            var model = properties.ToViewModel();
            model.ClientId = clientId;
            model.ClientName = CreateClientName(client.ClientId, client.ClientName);
            return model;
        }


        #endregion




        #region 客户端声明 ClientClaims


        public async Task<ClientClaimsViewModel> GetClientClaimAsync(int claimId)
        {
            var claim = await Repository.GetClientClaimAsync(claimId);
            if (claim == null) throw new NullResultException($"Invalid client claim id [{claimId}].");

            var client = await Repository.GetAsync(claim.Client.Id);
            if (client == null) throw new NullResultException($"Invalid client id [{claim.Client.Id}].");

            var model = claim.ToViewModel();
            model.ClientId = claim.Client.Id;
            model.ClientName = CreateClientName(client.ClientId, client.ClientName);
            return model;
        }

        public async Task<ClientClaimsViewModel> AddClientClaimAsync(ClientClaimsViewModel claims)
        {
            var entity = claims.ToEntity();
            var res = await Repository.AddClientClaimAsync(claims.ClientId, entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<ClientClaimsViewModel> RemoveClientClaimAsync(ClientClaimsViewModel claim)
        {
            var entity = claim.ToEntity();
            var res = await Repository.DeleteClientClaimAsync(entity);
            return res ? claim : null;
        }

        public async Task<ClientClaimsViewModel> GetClientClaimsAsync(int clientId, int page, int pageSize = 10)
        {
            var client = await Repository.GetAsync(clientId);
            if (client == null) throw new NullResultException($"Invalid client id [{clientId}].");

            var claims = await Repository.GetClientClaimsAsync(clientId, page, pageSize);
            var model = claims.ToViewModel();
            model.ClientId = clientId;
            model.ClientName = CreateClientName(client.ClientId, client.ClientName);
            return model;
        }

        #endregion





        #region 获取单个属性

        public async Task<List<string>> GetScopesAsync(string scope, int count = 0)
        {
            return await Repository.GetScopesAsync(scope, count);
        }


        public List<string> GetGrantTypes(string type, int count = 0)
        {
            return Repository.GetGrantTypes(type, count);
        }

        public List<string> GetSigningAlgorithms(string algorithm, int count = 0)
        {
            return Repository.GetSigningAlgorithms(algorithm, count);
        }

        public List<SelectItemViewModel> GetSecretTypes()
        {
            return Repository.GetSecretTypes().ToViewModel();
        }

        public virtual List<SelectItemViewModel> GetProtocolTypes()
        {
            return Repository.GetProtocolTypes().ToViewModel();
        }

        public List<string> GetStandardClaims(string claim, int count)
        {
            return Repository.GetStandardClaims(claim, count);
        }

        public virtual List<SelectItemViewModel> GetTokenUsages()
        {
            return Repository.GetTokenUsages().ToViewModel();
        }

        public virtual List<SelectItemViewModel> GetTokenExpirations()
        {
            return Repository.GetTokenExpirations().ToViewModel();
        }

        public List<SelectItemViewModel> GetTokenUsage()
        {
            return Repository.GetTokenUsages().ToViewModel();
        }

        public List<SelectItemViewModel> GetHashTypes()
        {
            return Repository.GetHashTypes().ToViewModel();
        }

        public virtual List<SelectItemViewModel> GetAccessTokenTypes()
        {
            return Repository.GetAccessTokenTypes().ToViewModel();
        }

        #endregion



        #region 特殊方法

        public Task<ClientViewModel> CloneClientAsync(ClientCloneViewModel client)
        {
            throw new System.NotImplementedException();
        }

        public ClientViewModel BuildClientViewModel(ClientViewModel client)
        {
            if (client == null)
            {
                return new ClientViewModel
                {
                    Id = 0,
                    AccessTokenTypes = GetAccessTokenTypes(),
                    RefreshTokenExpirations = GetTokenExpirations(),
                    RefreshTokenUsages = GetTokenUsages(),
                    ProtocolTypes = GetProtocolTypes()
                };
            }

            client.AccessTokenTypes = GetAccessTokenTypes();
            client.RefreshTokenExpirations = GetTokenExpirations();
            client.RefreshTokenUsages = GetTokenUsages();
            client.ProtocolTypes = GetProtocolTypes();

            client.AllowedScopes.AddRange(client.AllowedScopesItems.DeserializeSafety<List<string>>());
            client.PostLogoutRedirectUris.AddRange(client.PostLogoutRedirectUrisItems.DeserializeSafety<List<string>>());
            client.IdentityProviderRestrictions.AddRange(client.IdentityProviderRestrictionsItems.DeserializeSafety<List<string>>());
            client.RedirectUris.AddRange(client.RedirectUrisItems.DeserializeSafety<List<string>>());
            client.AllowedCorsOrigins.AddRange(client.AllowedCorsOriginsItems.DeserializeSafety<List<string>>());
            client.AllowedGrantTypes.AddRange(client.AllowedGrantTypesItems.DeserializeSafety<List<string>>());
            client.AllowedIdentityTokenSigningAlgorithms.AddRange(client.AllowedIdentityTokenSigningAlgorithmsItems
                .DeserializeSafety<List<string>>());

            return client;
        }

        public ClientSecretsViewModel BuildClientSecretsViewModel(ClientSecretsViewModel secrets)
        {
            secrets.HashTypes = GetHashTypes();
            secrets.TypeList = GetSecretTypes();
            return secrets;
        }


        #endregion




        #region 工具函数

        private static string CreateClientName(string id, string name)
        {
            return $"{name}({id})";
        }

        #endregion
    }
}