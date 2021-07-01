using Jeremy.IdentityCenter.Business.Models.ApiScopes;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Services.Interfaces
{
    public interface IApiScopeService : IBaseService
    {
        Task<ApiScopeViewModel> GetApiScopeAsync(int scopeId);

        Task<ApiScopesViewModel> GetApiScopesAsync(string search, int page = 1, int pageSize = 10);

        Task<ApiScopeViewModel> AddApiScopeAsync(ApiScopeViewModel scope);

        Task<ApiScopeViewModel> UpdateApiScopeAsync(ApiScopeViewModel scope);

        ApiScopeViewModel RemoveApiScope(ApiScopeViewModel scope);



        Task<ApiScopePropertiesViewModel> GetApiScopePropertyAsync(int propertyId);

        Task<ApiScopePropertiesViewModel> GetApiScopePropertiesAsync(int scopeId, int page, int pageSize = 10);

        Task<ApiScopePropertiesViewModel> AddApiScopePropertyAsync(ApiScopePropertiesViewModel properties);

        Task<ApiScopePropertiesViewModel> RemoveApiScopePropertyAsync(ApiScopePropertiesViewModel property);


        ApiScopeViewModel BuildApiScopeViewModel(ApiScopeViewModel model);
        Task<ApiScopePropertiesViewModel> BuildApiScopePropertiesViewModel(ApiScopePropertiesViewModel model);
    }
}
