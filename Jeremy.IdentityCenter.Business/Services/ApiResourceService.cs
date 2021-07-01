using IdentityServer4.Models;
using Jeremy.IdentityCenter.Business.Common.Models;
using Jeremy.IdentityCenter.Business.Mappers.Converts;
using Jeremy.IdentityCenter.Business.Models.ApiResources;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Jeremy.IdentityCenter.Database.Enums;
using Jeremy.IdentityCenter.Database.Repositories.Interfaces;
using Jeremy.Tools.Json;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Services
{
    public class ApiResourceService : BaseService<IApiResourceRepository, IApiResourceService>, IApiResourceService
    {
        public IClientService ClientService { get; }
        private const string SharedSecret = "SharedSecret";

        public ApiResourceService(IApiResourceRepository repository, ILogger<IApiResourceService> logger, IClientService clientService) : base(repository, logger)
        {
            ClientService = clientService;
        }

        public async Task<ApiResourceViewModel> GetApiResourceAsync(int resourceId)
        {
            var resource = await Repository.GetAsync(resourceId);
            if (resource == null) throw new NullResultException($"获取 Api 资源 {resourceId} 为空");

            return resource.ToViewModel();
        }

        public async Task<ApiResourcesViewModel> GetApiResourcesAsync(string search, int page = 1, int pageSize = 10)
        {
            return (await Repository.GetRangeAsync(
                x => string.IsNullOrWhiteSpace(search) || x.Name.Contains(search),
                page, pageSize)).ToViewModel();
        }

        public async Task<ApiResourceViewModel> AddApiResourceAsync(ApiResourceViewModel resource)
        {
            var entity = resource.ToEntity();
            if (await Repository.ExistApiResourceAsync(entity)) throw new NullResultException($"Can not create api resource [{resource.Id}].");

            var res = await Repository.AddAsync(entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<ApiResourceViewModel> UpdateApiResourceAsync(ApiResourceViewModel resource)
        {
            var entity = resource.ToEntity();
            if (await Repository.ExistApiResourceAsync(entity)) throw new NullResultException($"Can not update api resource [{resource.Id}].");

            var res = await Repository.UpdateAsync(entity);
            return res ? entity.ToViewModel() : null;
        }

        public ApiResourceViewModel RemoveApiResource(ApiResourceViewModel resource)
        {
            var res = Repository.Delete(resource.ToEntity());
            return res ? resource : null;
        }

        public async Task<ApiResourcePropertiesViewModel> GetApiResourcePropertyAsync(int propertyId)
        {
            var property = await Repository.GetApiResourcePropertyAsync(propertyId);
            if (property == null) throw new NullResultException($"Invalid api resource property id [{propertyId}].");

            var resource = await Repository.GetAsync(property.ApiResource.Id);
            if (resource == null) throw new NullResultException($"Invalid api resource id [{property.ApiResource.Id}].");

            var model = property.ToViewModel();
            model.ApiResourceId = property.ApiResourceId;
            model.ApiResourceName = resource.Name;
            return model;
        }

        public async Task<ApiResourcePropertiesViewModel> GetApiResourcePropertiesAsync(int resourceId, int page, int pageSize = 10)
        {
            var resource = await Repository.GetAsync(resourceId);
            if (resource == null) throw new NullResultException($"Invalid api resource id [{resourceId}].");

            var properties = await Repository.GetApiResourcePropertiesAsync(resourceId, page, pageSize);
            var model = properties.ToViewModel();
            model.ApiResourceId = resourceId;
            model.ApiResourceName = resource.Name;
            return model;
        }

        public async Task<ApiResourcePropertiesViewModel> AddApiResourcePropertyAsync(ApiResourcePropertiesViewModel properties)
        {
            var entity = properties.ToEntity();
            var res = await Repository.AddApiResourcePropertyAsync(properties.ApiResourceId, entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<ApiResourcePropertiesViewModel> RemoveApiResourcePropertyAsync(ApiResourcePropertiesViewModel property)
        {
            var res = await Repository.DeleteApiResourcePropertyAsync(property.ToEntity());
            return res ? property : null;
        }

        public async Task<ApiResourceSecretsViewModel> GetApiResourceSecretAsync(int secretId)
        {
            var secret = await Repository.GetApiResourceSecretAsync(secretId);
            if (secret == null) throw new NullResultException($"Invalid api resource secret id [{secretId}].");
            var model = secret.ToViewModel();

            model.Value = null;
            return model;
        }

        public async Task<ApiResourceSecretsViewModel> GetApiResourceSecretsAsync(int resourceId, int page = 1, int pageSize = 10)
        {
            var resource = await Repository.GetAsync(resourceId);
            if (resource == null) throw new NullResultException($"获取 Api 资源 {resourceId} 为空");

            var secrets = await Repository.GetApiResourceSecretsAsync(resourceId, page, pageSize);
            var model = secrets.ToViewModel();
            model.ApiResourceId = resourceId;
            model.ApiResourceName = resource.Name;

            model.ApiSecrets.ForEach(x => x.Value = null);
            return model;
        }

        public async Task<ApiResourceSecretsViewModel> AddApiResourceSecretAsync(ApiResourceSecretsViewModel secrets)
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
            var res = await Repository.AddApiResourceSecretAsync(secrets.ApiResourceId, entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<ApiResourceSecretsViewModel> RemoveApiResourceSecretAsync(ApiResourceSecretsViewModel secret)
        {
            var res = await Repository.DeleteApiResourceSecretAsync(secret.ToEntity());
            return res ? secret : null;
        }

        public ApiResourceViewModel BuildApiResourceViewModel(ApiResourceViewModel model)
        {
            model.UserClaims.AddRange(model.UserClaimsItems.DeserializeSafety<List<string>>());
            model.AllowedAccessTokenSigningAlgorithms.AddRange(model.AllowedAccessTokenSigningAlgorithmsItems.DeserializeSafety<List<string>>());
            model.Scopes.AddRange(model.ScopesItems.DeserializeSafety<List<string>>());
            return model;
        }

        public async Task<ApiResourcePropertiesViewModel> BuildApiResourcePropertiesViewModel(ApiResourcePropertiesViewModel model)
        {
            var properties = await GetApiResourcePropertiesAsync(model.ApiResourceId, 1);
            model.ApiResourceProperties.AddRange(properties.ApiResourceProperties);
            model.TotalCount = properties.TotalCount;
            return model;
        }

        public ApiResourceSecretsViewModel BuildApiResourceSecretsViewModel(ApiResourceSecretsViewModel model)
        {
            model.HashTypes = ClientService.GetHashTypes();
            model.TypeList = ClientService.GetSecretTypes();
            return model;
        }
    }
}