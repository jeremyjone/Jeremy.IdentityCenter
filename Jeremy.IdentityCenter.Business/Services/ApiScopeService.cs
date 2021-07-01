using Jeremy.IdentityCenter.Business.Common.Models;
using Jeremy.IdentityCenter.Business.Mappers.Extension;
using Jeremy.IdentityCenter.Business.Models.ApiScopes;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Jeremy.IdentityCenter.Database.Repositories.Interfaces;
using Jeremy.Tools.Json;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Services
{
    public class ApiScopeService : BaseService<IApiScopeRepository, IApiScopeService>, IApiScopeService
    {
        public ApiScopeService(IApiScopeRepository repository, ILogger<IApiScopeService> logger) : base(repository, logger)
        {
        }

        public async Task<ApiScopeViewModel> GetApiScopeAsync(int scopeId)
        {
            var scope = await Repository.GetAsync(scopeId);
            if (scope == null) throw new NullResultException($"获取 Api 作用域 {scopeId} 为空");

            return scope.ToViewModel();
        }

        public async Task<ApiScopesViewModel> GetApiScopesAsync(string search, int page = 1, int pageSize = 10)
        {
            return (await Repository.GetRangeAsync(
                x => string.IsNullOrWhiteSpace(search) || x.Name.Contains(search),
                page, pageSize)).ToViewModel();
        }

        public async Task<ApiScopeViewModel> AddApiScopeAsync(ApiScopeViewModel scope)
        {
            var entity = scope.ToEntity();
            if (await Repository.ExistApiScopeAsync(entity)) throw new NullResultException($"Can not create api scope [{scope.Id}].");

            var res = await Repository.AddAsync(entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<ApiScopeViewModel> UpdateApiScopeAsync(ApiScopeViewModel scope)
        {
            var entity = scope.ToEntity();
            if (await Repository.ExistApiScopeAsync(entity)) throw new NullResultException($"Can not update api scope [{scope.Id}].");

            var res = await Repository.UpdateAsync(entity);
            return res ? entity.ToViewModel() : null;
        }

        public ApiScopeViewModel RemoveApiScope(ApiScopeViewModel scope)
        {
            var res = Repository.Delete(scope.ToEntity());
            return res ? scope : null;
        }

        public async Task<ApiScopePropertiesViewModel> GetApiScopePropertyAsync(int propertyId)
        {
            var property = await Repository.GetApiScopePropertyAsync(propertyId);
            if (property == null) throw new NullResultException($"Invalid api scope property id [{propertyId}].");

            var scope = await Repository.GetAsync(property.Scope.Id);
            if (scope == null) throw new NullResultException($"Invalid api scope id [{property.Scope.Id}].");

            var model = property.ToViewModel();
            model.ApiScopeId = property.ScopeId;
            model.ApiScopeName = scope.Name;
            return model;
        }

        public async Task<ApiScopePropertiesViewModel> GetApiScopePropertiesAsync(int scopeId, int page, int pageSize = 10)
        {
            var scope = await Repository.GetAsync(scopeId);
            if (scope == null) throw new NullResultException($"Invalid api scope id [{scopeId}].");

            var properties = await Repository.GetApiScopePropertiesAsync(scopeId, page, pageSize);
            var model = properties.ToViewModel();
            model.ApiScopeId = scopeId;
            model.ApiScopeName = scope.Name;
            return model;
        }

        public async Task<ApiScopePropertiesViewModel> AddApiScopePropertyAsync(ApiScopePropertiesViewModel properties)
        {
            var entity = properties.ToEntity();
            var res = await Repository.AddApiScopePropertyAsync(properties.ApiScopeId, entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<ApiScopePropertiesViewModel> RemoveApiScopePropertyAsync(ApiScopePropertiesViewModel property)
        {
            var res = await Repository.DeleteApiScopePropertyAsync(property.ToEntity());
            return res ? property : null;
        }

        public ApiScopeViewModel BuildApiScopeViewModel(ApiScopeViewModel model)
        {
            model.UserClaims.AddRange(model.UserClaimsItems.DeserializeSafety<List<string>>());
            return model;
        }

        public async Task<ApiScopePropertiesViewModel> BuildApiScopePropertiesViewModel(ApiScopePropertiesViewModel model)
        {
            var properties = await GetApiScopePropertiesAsync(model.ApiScopeId, 1);
            model.ApiScopeProperties.AddRange(properties.ApiScopeProperties);
            model.TotalCount = properties.TotalCount;
            return model;
        }
    }
}