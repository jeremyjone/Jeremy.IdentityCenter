// Based on the IdentityServer4.EntityFramework - authors - Brock Allen & Dominick Baier.
// https://github.com/IdentityServer/IdentityServer4.EntityFramework

// Modified by Jan Škoruba

// Modified by JeremyJone

using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Business.Mappers.Converts;
using Jeremy.IdentityCenter.Business.Models;
using Jeremy.IdentityCenter.Business.Models.Clients;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Models;

namespace Jeremy.IdentityCenter.Business.Mappers
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            // entity to model
            CreateMap<Client, ClientViewModel>(MemberList.Destination)
                .ForMember(dest => dest.ProtocolType, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.AllowedIdentityTokenSigningAlgorithms,
                    opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter,
                        x => x.AllowedIdentityTokenSigningAlgorithms))
                .ReverseMap()
                .ForMember(x => x.AllowedIdentityTokenSigningAlgorithms,
                    opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter,
                        x => x.AllowedIdentityTokenSigningAlgorithms));

            CreateMap<SelectItem, SelectItemViewModel>(MemberList.Destination)
                .ReverseMap();

            CreateMap<ClientGrantType, string>()
                .ConstructUsing(src => src.GrantType)
                .ReverseMap()
                .ForMember(dest => dest.GrantType, opt => opt.MapFrom(src => src));

            CreateMap<ClientRedirectUri, string>()
                .ConstructUsing(src => src.RedirectUri)
                .ReverseMap()
                .ForMember(dest => dest.RedirectUri, opt => opt.MapFrom(src => src));

            CreateMap<ClientPostLogoutRedirectUri, string>()
                .ConstructUsing(src => src.PostLogoutRedirectUri)
                .ReverseMap()
                .ForMember(dest => dest.PostLogoutRedirectUri, opt => opt.MapFrom(src => src));

            CreateMap<ClientScope, string>()
                .ConstructUsing(src => src.Scope)
                .ReverseMap()
                .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));

            CreateMap<ClientSecret, ClientSecretViewModel>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();

            CreateMap<ClientClaim, ClientClaimViewModel>(MemberList.None)
                .ConstructUsing(src => new ClientClaimViewModel { Type = src.Type, Value = src.Value })
                .ReverseMap();

            CreateMap<ClientIdPRestriction, string>()
                .ConstructUsing(src => src.Provider)
                .ReverseMap()
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src));

            CreateMap<ClientCorsOrigin, string>()
                .ConstructUsing(src => src.Origin)
                .ReverseMap()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src));

            CreateMap<ClientProperty, ClientPropertyViewModel>(MemberList.Destination)
                .ReverseMap();




            CreateMap<ClientSecret, ClientSecretsViewModel>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.ClientSecretId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.ClientId, opt => opt.MapFrom(x => x.Client.Id));

            CreateMap<ClientClaim, ClientClaimsViewModel>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.ClientClaimId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.ClientId, opt => opt.MapFrom(x => x.Client.Id));

            CreateMap<ClientProperty, ClientPropertiesViewModel>(MemberList.Destination)
                .ForMember(dest => dest.Key, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.ClientPropertyId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.ClientId, opt => opt.MapFrom(x => x.Client.Id));

            //PagedLists
            CreateMap<PageList<ClientSecret>, ClientSecretsViewModel>(MemberList.Destination)
                .ForMember(x => x.ClientSecrets, opt => opt.MapFrom(src => src.Items));

            CreateMap<PageList<ClientClaim>, ClientClaimsViewModel>(MemberList.Destination)
                .ForMember(x => x.ClientClaims, opt => opt.MapFrom(src => src.Items));

            CreateMap<PageList<ClientProperty>, ClientPropertiesViewModel>(MemberList.Destination)
                .ForMember(x => x.ClientProperties, opt => opt.MapFrom(src => src.Items));

            CreateMap<PageList<Client>, ClientsViewModel>(MemberList.Destination)
                .ForMember(x => x.Clients, opt => opt.MapFrom(src => src.Items));

            // model to entity
            CreateMap<ClientSecretsViewModel, ClientSecret>(MemberList.Source)
                .ForMember(x => x.Client, dto => dto.MapFrom(src => new Client() { Id = src.ClientId }))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ClientSecretId));

            CreateMap<ClientClaimsViewModel, ClientClaim>(MemberList.Source)
                .ForMember(x => x.Client, dto => dto.MapFrom(src => new Client() { Id = src.ClientId }))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ClientClaimId));

            CreateMap<ClientPropertiesViewModel, ClientProperty>(MemberList.Source)
                .ForMember(x => x.Client, dto => dto.MapFrom(src => new Client() { Id = src.ClientId }))
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.ClientPropertyId));
        }
    }
}
