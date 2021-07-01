using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Business.Models.IdentityResource;
using Jeremy.IdentityCenter.Database.Common.Models;
using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Mappers.Extension
{
    public static class IdentityResourceExtension
    {
        static IdentityResourceExtension()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityResourceProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }


        public static IdentityResourceViewModel ToViewModel(this IdentityResource resource)
        {
            return resource == null ? null : Mapper.Map<IdentityResourceViewModel>(resource);
        }

        public static IdentityResourcesViewModel ToViewModel(this PageList<IdentityResource> resource)
        {
            return resource == null ? null : Mapper.Map<IdentityResourcesViewModel>(resource);
        }

        public static List<IdentityResourceViewModel> ToViewModel(this List<IdentityResource> resource)
        {
            return resource == null ? null : Mapper.Map<List<IdentityResourceViewModel>>(resource);
        }

        public static IdentityResource ToEntity(this IdentityResourceViewModel resource)
        {
            return resource == null ? null : Mapper.Map<IdentityResource>(resource);
        }

        public static IdentityResourcePropertiesViewModel ToViewModel(this PageList<IdentityResourceProperty> identityResourceProperties)
        {
            return Mapper.Map<IdentityResourcePropertiesViewModel>(identityResourceProperties);
        }

        public static IdentityResourcePropertiesViewModel ToViewModel(this IdentityResourceProperty identityResourceProperty)
        {
            return Mapper.Map<IdentityResourcePropertiesViewModel>(identityResourceProperty);
        }

        public static List<IdentityResource> ToEntity(this List<IdentityResourceViewModel> resource)
        {
            return resource == null ? null : Mapper.Map<List<IdentityResource>>(resource);
        }

        public static IdentityResourceProperty ToEntity(this IdentityResourcePropertiesViewModel identityResourceProperties)
        {
            return Mapper.Map<IdentityResourceProperty>(identityResourceProperties);
        }
    }
}