using Jeremy.IdentityCenter.Business.Models.IdentityResource;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Services.Interfaces
{
    public interface IIdentityResourceService : IBaseService
    {
        Task<IdentityResourceViewModel> GetIdentityResourceAsync(int resourceId);

        Task<IdentityResourcesViewModel> GetIdentityResourcesAsync(string search, int page = 1, int pageSize = 10);

        Task<IdentityResourceViewModel> AddIdentityResourceAsync(IdentityResourceViewModel resource);

        Task<IdentityResourceViewModel> UpdateIdentityResourceAsync(IdentityResourceViewModel resource);

        IdentityResourceViewModel RemoveIdentityResource(IdentityResourceViewModel resource);



        Task<IdentityResourcePropertiesViewModel> GetIdentityResourcePropertyAsync(int propertyId);

        Task<IdentityResourcePropertiesViewModel> GetIdentityResourcePropertiesAsync(int resourceId, int page, int pageSize = 10);

        Task<IdentityResourcePropertiesViewModel> AddIdentityResourcePropertyAsync(IdentityResourcePropertiesViewModel properties);

        Task<IdentityResourcePropertiesViewModel> RemoveIdentityResourcePropertyAsync(IdentityResourcePropertiesViewModel property);


        IdentityResourceViewModel BuildIdentityResourceViewModel(IdentityResourceViewModel model);
    }
}
