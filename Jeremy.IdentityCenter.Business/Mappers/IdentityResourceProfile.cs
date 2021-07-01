// Based on the IdentityServer4.EntityFramework - authors - Brock Allen & Dominick Baier.
// https://github.com/IdentityServer/IdentityServer4.EntityFramework

// Modified by Jan Škoruba

// Modified by JeremyJone

using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Business.Models.IdentityResource;
using Jeremy.IdentityCenter.Database.Common.Models;
using System.Linq;

namespace Jeremy.IdentityCenter.Business.Mappers
{
    public class IdentityResourceProfile : Profile
    {
        public IdentityResourceProfile()
        {
            // entity to model
            CreateMap<IdentityResource, IdentityResourceViewModel>(MemberList.Destination)
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(src => src.UserClaims.Select(x => x.Type)));

            CreateMap<IdentityResourceProperty, IdentityResourcePropertyViewModel>(MemberList.Destination)
                .ReverseMap();

            CreateMap<IdentityResourceProperty, IdentityResourcePropertiesViewModel>(MemberList.Destination)
                .ForMember(dest => dest.Key, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.IdentityResourcePropertyId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.IdentityResourceId, opt => opt.MapFrom(x => x.IdentityResource.Id));

            CreateMap<PageList<IdentityResource>, IdentityResourcesViewModel>(MemberList.Destination)
                .ForMember(x => x.IdentityResources,
                    opt => opt.MapFrom(src => src.Items));

            CreateMap<PageList<IdentityResourceProperty>, IdentityResourcePropertiesViewModel>(MemberList.Destination)
                .ForMember(x => x.IdentityResourceProperties, opt => opt.MapFrom(src => src.Items));

            // model to entity
            CreateMap<IdentityResourceViewModel, IdentityResource>(MemberList.Source)
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => new IdentityResourceClaim { Type = x })));

            CreateMap<IdentityResourcePropertiesViewModel, IdentityResourceProperty>(MemberList.Source)
                .ForMember(x => x.IdentityResource, dto => dto.MapFrom(src => new IdentityResource() { Id = src.IdentityResourceId }))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.IdentityResourcePropertyId));
        }
    }
}