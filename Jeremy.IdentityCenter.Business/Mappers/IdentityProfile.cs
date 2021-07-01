using AutoMapper;
using Jeremy.IdentityCenter.Business.Models.Account;
using Jeremy.IdentityCenter.Business.Models.Identity;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Mappers
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            // entity to model
            CreateMap<IdcIdentityUser, UserViewModel>(MemberList.Destination);

            CreateMap<UserLoginInfo, UserProviderViewModel>(MemberList.Destination);

            // entity to model
            CreateMap<IdcIdentityRole, RoleViewModel>(MemberList.Destination);

            CreateMap<IdcIdentityUser, IdcIdentityUser>(MemberList.Destination)
                .ForMember(x => x.SecurityStamp, opt => opt.Ignore());

            CreateMap<IdcIdentityRole, IdcIdentityRole>(MemberList.Destination);

            CreateMap<PageList<IdcIdentityUser>, UsersViewModel>(MemberList.Destination)
                .ForMember(x => x.Users,
                    opt => opt.MapFrom(src => src.Items));

            CreateMap<IdentityUserClaim<int>, UserClaimViewModel>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<IdentityUserClaim<int>, UserClaimsViewModel>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<PageList<IdcIdentityRole>, RolesViewModel>(MemberList.Destination)
                .ForMember(x => x.Roles,
                    opt => opt.MapFrom(src => src.Items));

            CreateMap<PageList<IdcIdentityRole>, UserRolesViewModel>(MemberList.Destination)
                .ForMember(x => x.Roles,
                    opt => opt.MapFrom(src => src.Items));

            CreateMap<PageList<IdentityUserClaim<int>>, UserClaimsViewModel>(MemberList.Destination)
                .ForMember(x => x.Claims,
                    opt => opt.MapFrom(src => src.Items));

            CreateMap<PageList<IdentityRoleClaim<int>>, RoleClaimsViewModel>(MemberList.Destination)
                .ForMember(x => x.Claims,
                    opt => opt.MapFrom(src => src.Items));

            CreateMap<List<UserLoginInfo>, UserProvidersViewModel>(MemberList.Destination)
                .ForMember(x => x.Providers, opt => opt.MapFrom(src => src));

            CreateMap<UserLoginInfo, UserProviderViewModel>(MemberList.Destination);

            CreateMap<IdentityRoleClaim<int>, RoleClaimViewModel>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<IdentityRoleClaim<int>, RoleClaimsViewModel>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<IdentityUserLogin<int>, UserProviderViewModel>(MemberList.Destination);

            // model to entity
            CreateMap<RoleViewModel, IdcIdentityRole>(MemberList.Source)
                .ForMember(dest => dest.Id, opt => opt.Condition(srs => srs.Id != null)); ;

            CreateMap<RoleClaimsViewModel, IdentityRoleClaim<int>>(MemberList.Source);

            CreateMap<UserClaimsViewModel, IdentityUserClaim<int>>(MemberList.Source)
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(src => src.ClaimId));

            // model to entity
            CreateMap<UserViewModel, IdcIdentityUser>(MemberList.Source)
                .ForMember(dest => dest.Id, opt => opt.Condition(srs => srs.Id != null)); ;

            // model to entity
            CreateMap<RegisterViewModel, IdcIdentityUser>(MemberList.Source)
                .ForMember(dest => dest.Sex,
                    opt => opt.Ignore());
        }
    }
}
