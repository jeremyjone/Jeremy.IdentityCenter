using Jeremy.IdentityCenter.Business.Models.ApiResources;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Services.Interfaces
{
    public interface IApiResourceService : IBaseService
    {
        Task<ApiResourceViewModel> GetApiResourceAsync(int resourceId);

        Task<ApiResourcesViewModel> GetApiResourcesAsync(string search, int page = 1, int pageSize = 10);

        Task<ApiResourceViewModel> AddApiResourceAsync(ApiResourceViewModel resource);

        Task<ApiResourceViewModel> UpdateApiResourceAsync(ApiResourceViewModel resource);

        ApiResourceViewModel RemoveApiResource(ApiResourceViewModel resource);



        Task<ApiResourcePropertiesViewModel> GetApiResourcePropertyAsync(int propertyId);

        Task<ApiResourcePropertiesViewModel> GetApiResourcePropertiesAsync(int resourceId, int page = 1, int pageSize = 10);

        Task<ApiResourcePropertiesViewModel> AddApiResourcePropertyAsync(ApiResourcePropertiesViewModel properties);

        Task<ApiResourcePropertiesViewModel> RemoveApiResourcePropertyAsync(ApiResourcePropertiesViewModel property);



        Task<ApiResourceSecretsViewModel> GetApiResourceSecretAsync(int secretId);

        Task<ApiResourceSecretsViewModel> GetApiResourceSecretsAsync(int resourceId, int page = 1, int pageSize = 10);

        Task<ApiResourceSecretsViewModel> AddApiResourceSecretAsync(ApiResourceSecretsViewModel secrets);

        Task<ApiResourceSecretsViewModel> RemoveApiResourceSecretAsync(ApiResourceSecretsViewModel secret);



        ApiResourceViewModel BuildApiResourceViewModel(ApiResourceViewModel model);
        Task<ApiResourcePropertiesViewModel> BuildApiResourcePropertiesViewModel(ApiResourcePropertiesViewModel model);

        ApiResourceSecretsViewModel BuildApiResourceSecretsViewModel(ApiResourceSecretsViewModel model);
    }
}
