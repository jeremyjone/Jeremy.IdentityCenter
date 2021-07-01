using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Business.Models.ApiScopes;
using Jeremy.IdentityCenter.Database.Common.Models;
using System.Linq;

namespace Jeremy.IdentityCenter.Business.Mappers
{
    public class ApiScopeProfile : Profile
    {
        public ApiScopeProfile()
        {
            // entity to model
            CreateMap<ApiScope, ApiScopeViewModel>(MemberList.Destination)
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(src => src.UserClaims.Select(x => x.Type)));

            CreateMap<ApiScopeProperty, ApiScopePropertyViewModel>(MemberList.Destination)
                .ReverseMap();

            CreateMap<ApiScopeProperty, ApiScopePropertiesViewModel>(MemberList.Destination)
                .ForMember(dest => dest.Key, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.ApiScopePropertyId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.ApiScopeId, opt => opt.MapFrom(x => x.Scope.Id));

            // PagedLists
            CreateMap<PageList<ApiScope>, ApiScopesViewModel>(MemberList.Destination)
                .ForMember(x => x.Scopes, opt => opt.MapFrom(src => src.Items));

            CreateMap<PageList<ApiScopeProperty>, ApiScopePropertiesViewModel>(MemberList.Destination)
                .ForMember(x => x.ApiScopeProperties, opt => opt.MapFrom(src => src.Items));

            // model to entity
            CreateMap<ApiScopeViewModel, ApiScope>(MemberList.Source)
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => new ApiScopeClaim { Type = x })));

            CreateMap<ApiScopePropertiesViewModel, ApiScopeProperty>(MemberList.Source)
                .ForMember(x => x.Scope, dto => dto.MapFrom(src => new ApiScope { Id = src.ApiScopeId }))
                .ForMember(x => x.ScopeId, dto => dto.MapFrom(src => src.ApiScopeId))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ApiScopePropertyId));
        }
    }
}
