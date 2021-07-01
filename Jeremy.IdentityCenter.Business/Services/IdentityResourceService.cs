using Jeremy.IdentityCenter.Business.Common.Models;
using Jeremy.IdentityCenter.Business.Mappers.Extension;
using Jeremy.IdentityCenter.Business.Models.IdentityResource;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Jeremy.IdentityCenter.Database.Repositories.Interfaces;
using Jeremy.Tools.Json;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Services
{
    public class IdentityResourceService : BaseService<IIdentityResourceRepository, IIdentityResourceService>, IIdentityResourceService
    {
        public IdentityResourceService(IIdentityResourceRepository repository, ILogger<IIdentityResourceService> logger) : base(repository, logger)
        {
        }

        public async Task<IdentityResourceViewModel> GetIdentityResourceAsync(int resourceId)
        {
            var resource = await Repository.GetAsync(resourceId);
            if (resource == null) throw new NullResultException($"获取身份资源 {resourceId} 为空");

            return resource.ToViewModel();
        }

        public async Task<IdentityResourcesViewModel> GetIdentityResourcesAsync(string search, int page = 1, int pageSize = 10)
        {
            return (await Repository.GetRangeAsync(
                x => string.IsNullOrWhiteSpace(search) || x.Name.Contains(search),
                page, pageSize)).ToViewModel();
        }

        public async Task<IdentityResourceViewModel> AddIdentityResourceAsync(IdentityResourceViewModel resource)
        {
            var entity = resource.ToEntity();
            if (await Repository.ExistIdentityResourceAsync(entity)) throw new NullResultException($"Can not create identity resource [{resource.Id}].");

            var res = await Repository.AddAsync(entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<IdentityResourceViewModel> UpdateIdentityResourceAsync(IdentityResourceViewModel resource)
        {
            var entity = resource.ToEntity();
            if (await Repository.ExistIdentityResourceAsync(entity)) throw new NullResultException($"Can not update identity resource [{resource.Id}].");

            var res = await Repository.UpdateAsync(entity);
            return res ? entity.ToViewModel() : null;
        }

        public IdentityResourceViewModel RemoveIdentityResource(IdentityResourceViewModel resource)
        {
            var res = Repository.Delete(resource.ToEntity());
            return res ? resource : null;
        }

        public async Task<IdentityResourcePropertiesViewModel> GetIdentityResourcePropertyAsync(int propertyId)
        {
            var property = await Repository.GetIdentityResourcePropertyAsync(propertyId);
            if (property == null) throw new NullResultException($"Invalid identity resource property id [{propertyId}].");

            var resource = await Repository.GetAsync(property.IdentityResource.Id);
            if (resource == null) throw new NullResultException($"Invalid identity resource id [{property.IdentityResource.Id}].");

            var model = property.ToViewModel();
            model.IdentityResourceId = property.IdentityResourceId;
            model.IdentityResourceName = resource.Name;
            return model;
        }

        public async Task<IdentityResourcePropertiesViewModel> GetIdentityResourcePropertiesAsync(int resourceId, int page, int pageSize = 10)
        {
            var resource = await Repository.GetAsync(resourceId);
            if (resource == null) throw new NullResultException($"Invalid identity resource id [{resourceId}].");

            var properties = await Repository.GetIdentityResourcePropertiesAsync(resourceId, page, pageSize);
            var model = properties.ToViewModel();
            model.IdentityResourceId = resourceId;
            model.IdentityResourceName = resource.Name;
            return model;
        }

        public async Task<IdentityResourcePropertiesViewModel> AddIdentityResourcePropertyAsync(IdentityResourcePropertiesViewModel properties)
        {
            var entity = properties.ToEntity();
            var res = await Repository.AddIdentityResourcePropertyAsync(properties.IdentityResourceId, entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<IdentityResourcePropertiesViewModel> RemoveIdentityResourcePropertyAsync(IdentityResourcePropertiesViewModel property)
        {
            var res = await Repository.DeleteIdentityResourcePropertyAsync(property.ToEntity());
            return res ? property : null;
        }

        public IdentityResourceViewModel BuildIdentityResourceViewModel(IdentityResourceViewModel model)
        {
            model.UserClaims.AddRange(model.UserClaimsItems.DeserializeSafety<List<string>>());
            return model;
        }
    }
}