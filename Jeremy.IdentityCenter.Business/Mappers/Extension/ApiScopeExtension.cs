using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Business.Models.ApiScopes;
using Jeremy.IdentityCenter.Database.Common.Models;

namespace Jeremy.IdentityCenter.Business.Mappers.Extension
{
    public static class ApiScopeExtension
    {
        internal static IMapper Mapper { get; }

        static ApiScopeExtension()
        {
            Mapper = new MapperConfiguration(config => config.AddProfile<ApiScopeProfile>()).CreateMapper();
        }

        public static ApiScopesViewModel ToViewModel(this PageList<ApiScope> scopes)
        {
            return scopes == null ? null : Mapper.Map<ApiScopesViewModel>(scopes);
        }

        public static ApiScopeViewModel ToViewModel(this ApiScope resource)
        {
            return resource == null ? null : Mapper.Map<ApiScopeViewModel>(resource);
        }

        public static ApiScope ToEntity(this ApiScopeViewModel scope)
        {
            return scope == null ? null : Mapper.Map<ApiScope>(scope);
        }

        public static ApiScopeProperty ToEntity(this ApiScopePropertiesViewModel scope)
        {
            return scope == null ? null : Mapper.Map<ApiScopeProperty>(scope);
        }

        public static ApiScopePropertiesViewModel ToViewModel(this PageList<ApiScopeProperty> scope)
        {
            return scope == null ? null : Mapper.Map<ApiScopePropertiesViewModel>(scope);
        }

        public static ApiScopePropertiesViewModel ToViewModel(this ApiScopeProperty scope)
        {
            return scope == null ? null : Mapper.Map<ApiScopePropertiesViewModel>(scope);
        }
    }
}
