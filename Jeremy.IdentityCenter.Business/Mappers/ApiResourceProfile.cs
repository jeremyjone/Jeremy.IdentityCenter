using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Business.Mappers.Converts;
using Jeremy.IdentityCenter.Business.Models.ApiResources;
using Jeremy.IdentityCenter.Database.Common.Models;
using System.Linq;

namespace Jeremy.IdentityCenter.Business.Mappers
{
    public class ApiResourceProfile : Profile
    {
        public ApiResourceProfile()
        {
            // entity to model
            CreateMap<ApiResource, ApiResourceViewModel>(MemberList.Destination)
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => x.Type)))
                .ForMember(x => x.Scopes, opts => opts.MapFrom(src => src.Scopes.Select(x => x.Scope)))
                .ForMember(x => x.AllowedAccessTokenSigningAlgorithms,
                    opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter,
                        x => x.AllowedAccessTokenSigningAlgorithms));

            CreateMap<ApiResourceSecret, ApiResourceSecretsViewModel>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.ApiSecretId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.ApiResourceId, opt => opt.MapFrom(x => x.ApiResource.Id));

            CreateMap<ApiResourceSecret, ApiResourceSecretViewModel>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null));

            CreateMap<ApiResourceProperty, ApiResourcePropertyViewModel>(MemberList.Destination)
                .ReverseMap();

            CreateMap<ApiResourceProperty, ApiResourcePropertiesViewModel>(MemberList.Destination)
                .ForMember(dest => dest.Key, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.ApiResourcePropertyId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.ApiResourceId, opt => opt.MapFrom(x => x.ApiResource.Id));

            //PagedLists
            CreateMap<PageList<ApiResource>, ApiResourcesViewModel>(MemberList.Destination)
                .ForMember(x => x.Resources, opt => opt.MapFrom(src => src.Items));

            CreateMap<PageList<ApiResourceSecret>, ApiResourceSecretsViewModel>(MemberList.Destination)
                .ForMember(x => x.ApiSecrets, opt => opt.MapFrom(src => src.Items));

            CreateMap<PageList<ApiResourceProperty>, ApiResourcePropertiesViewModel>(MemberList.Destination)
                .ForMember(x => x.ApiResourceProperties, opt => opt.MapFrom(src => src.Items));

            // model to entity
            CreateMap<ApiResourceViewModel, ApiResource>(MemberList.Source)
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => new ApiResourceClaim { Type = x })))
                .ForMember(x => x.Scopes, opts => opts.MapFrom(src => src.Scopes.Select(x => new ApiResourceScope { Scope = x })))
                .ForMember(x => x.AllowedAccessTokenSigningAlgorithms,
                    opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter,
                        x => x.AllowedAccessTokenSigningAlgorithms));

            CreateMap<ApiResourceSecretsViewModel, ApiResourceSecret>(MemberList.Source)
                .ForMember(x => x.ApiResource, opts => opts.MapFrom(src => new ApiResource() { Id = src.ApiResourceId }))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ApiSecretId));

            CreateMap<ApiResourcePropertiesViewModel, ApiResourceProperty>(MemberList.Source)
                .ForMember(x => x.ApiResource, dto => dto.MapFrom(src => new ApiResource() { Id = src.ApiResourceId }))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ApiResourcePropertyId));
        }
    }
}
