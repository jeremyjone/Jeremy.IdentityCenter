using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Business.Models.PersistedGrants;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Entities;

namespace Jeremy.IdentityCenter.Business.Mappers.Extension
{
    public static class PersistedGrantExtension
    {
        static PersistedGrantExtension()
        {
            Mapper = new MapperConfiguration(config => config.AddProfile<PersistedGrantProfile>()).CreateMapper();
        }

        internal static IMapper Mapper { get; set; }

        public static PersistedGrantsViewModel ToViewModel(this PageList<PersistedGrantData> grant)
        {
            return grant == null ? null : Mapper.Map<PersistedGrantsViewModel>(grant);
        }

        public static PersistedGrantsViewModel ToViewModel(this PageList<PersistedGrant> grant)
        {
            return grant == null ? null : Mapper.Map<PersistedGrantsViewModel>(grant);
        }

        public static PersistedGrantViewModel ToViewModel(this PersistedGrant grant)
        {
            return grant == null ? null : Mapper.Map<PersistedGrantViewModel>(grant);
        }
    }
}
