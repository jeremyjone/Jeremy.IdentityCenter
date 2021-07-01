using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Business.Models.ApiResources;
using Jeremy.IdentityCenter.Database.Common.Models;

namespace Jeremy.IdentityCenter.Business.Mappers.Converts
{
    public static class ApiResourceExtension
    {
        static ApiResourceExtension()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static ApiResourceViewModel ToViewModel(this ApiResource resource)
        {
            return resource == null ? null : Mapper.Map<ApiResourceViewModel>(resource);
        }

        public static ApiResourcesViewModel ToViewModel(this PageList<ApiResource> resources)
        {
            return resources == null ? null : Mapper.Map<ApiResourcesViewModel>(resources);
        }

        public static ApiResourcePropertiesViewModel ToViewModel(this PageList<ApiResourceProperty> apiResourceProperties)
        {
            return Mapper.Map<ApiResourcePropertiesViewModel>(apiResourceProperties);
        }

        public static ApiResourcePropertiesViewModel ToViewModel(this ApiResourceProperty apiResourceProperty)
        {
            return Mapper.Map<ApiResourcePropertiesViewModel>(apiResourceProperty);
        }

        public static ApiResourceSecretsViewModel ToViewModel(this PageList<ApiResourceSecret> secrets)
        {
            return secrets == null ? null : Mapper.Map<ApiResourceSecretsViewModel>(secrets);
        }

        public static ApiResourceSecretsViewModel ToViewModel(this ApiResourceSecret resource)
        {
            return resource == null ? null : Mapper.Map<ApiResourceSecretsViewModel>(resource);
        }

        public static ApiResource ToEntity(this ApiResourceViewModel resource)
        {
            return resource == null ? null : Mapper.Map<ApiResource>(resource);
        }

        public static ApiResourceSecret ToEntity(this ApiResourceSecretsViewModel resource)
        {
            return resource == null ? null : Mapper.Map<ApiResourceSecret>(resource);
        }

        public static ApiResourceProperty ToEntity(this ApiResourcePropertiesViewModel apiResourceProperties)
        {
            return Mapper.Map<ApiResourceProperty>(apiResourceProperties);
        }
    }
}
