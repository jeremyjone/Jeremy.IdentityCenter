using AutoMapper;
using Jeremy.IdentityCenter.Business.Models.Identity;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Mappers.Extension
{
    public static class IdentityExtension
    {
        internal static IMapper Mapper { get; }

        static IdentityExtension()
        {
            Mapper = new MapperConfiguration(config => config.AddProfile<IdentityProfile>()).CreateMapper();
        }

        public static UsersViewModel ToViewModel(this PageList<IdcIdentityUser> users)
        {
            return Mapper.Map<UsersViewModel>(users);
        }

        public static UserViewModel ToViewModel(this IdcIdentityUser user)
        {
            return Mapper.Map<UserViewModel>(user);
        }

        public static IdcIdentityUser ToEntity(this UserViewModel model)
        {
            return Mapper.Map<IdcIdentityUser>(model);
        }



        public static RoleViewModel ToViewModel(this IdcIdentityRole role)
        {
            return Mapper.Map<RoleViewModel>(role);
        }

        public static List<RoleViewModel> ToViewModel(this List<IdcIdentityRole> roles)
        {
            return Mapper.Map<List<RoleViewModel>>(roles);
        }

        public static T ToViewModel<T>(this PageList<IdcIdentityRole> roles) where T : IBaseViewModel
        {
            return Mapper.Map<T>(roles);
        }

        public static IdcIdentityRole ToEntity(this RoleViewModel model)
        {
            return Mapper.Map<IdcIdentityRole>(model);
        }



        public static UserClaimsViewModel ToViewModel(this IdentityUserClaim<int> claim)
        {
            return Mapper.Map<UserClaimsViewModel>(claim);
        }

        public static T ToViewModel<T>(this PageList<IdentityUserClaim<int>> claim) where T : IBaseViewModel
        {
            return Mapper.Map<T>(claim);
        }

        public static IdentityUserClaim<int> ToEntity(this UserClaimsViewModel model)
        {
            return Mapper.Map<IdentityUserClaim<int>>(model);
        }




        public static UserProviderViewModel ToViewModel(this IdentityUserLogin<int> login)
        {
            return Mapper.Map<UserProviderViewModel>(login);
        }

        public static UserProvidersViewModel ToViewModel(this List<UserLoginInfo> infos)
        {
            return Mapper.Map<UserProvidersViewModel>(infos);
        }


        public static RoleClaimsViewModel ToViewModel(this PageList<IdentityRoleClaim<int>> claim)
        {
            return Mapper.Map<RoleClaimsViewModel>(claim);
        }

        public static RoleClaimsViewModel ToViewModel(this IdentityRoleClaim<int> claim)
        {
            return Mapper.Map<RoleClaimsViewModel>(claim);
        }

        public static IdentityRoleClaim<int> ToEntity(this RoleClaimsViewModel claim)
        {
            return Mapper.Map<IdentityRoleClaim<int>>(claim);
        }
    }
}
