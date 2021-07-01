using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Business.Models.PersistedGrants;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Entities;

namespace Jeremy.IdentityCenter.Business.Mappers
{
    public class PersistedGrantProfile : Profile
    {
        public PersistedGrantProfile()
        {
            // entity to model
            CreateMap<PersistedGrant, PersistedGrantViewModel>(MemberList.Destination)
                .ReverseMap();

            CreateMap<PersistedGrantData, PersistedGrantViewModel>(MemberList.Destination);

            CreateMap<PageList<PersistedGrant>, PersistedGrantsViewModel>(MemberList.Destination)
                .ForMember(x => x.PersistedGrants,
                    opt => opt.MapFrom(src => src.Items));

            CreateMap<PageList<PersistedGrantData>, PersistedGrantsViewModel>(MemberList.Destination)
                .ForMember(x => x.PersistedGrants,
                    opt => opt.MapFrom(src => src.Items));
        }
    }
}
