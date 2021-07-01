using Jeremy.IdentityCenter.Business.Models;
using Jeremy.IdentityCenter.Business.Models.Account;
using Jeremy.IdentityCenter.Business.Models.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Services.Interfaces
{
    public interface IIdentityService : IBaseService
    {
        Task<UserViewModel> GetUserAsync(int userId);

        Task<UsersViewModel> GetUsersAsync(string search, int page = 1, int pageSize = 10);

        Task<UserViewModel> AddUserAsync(UserViewModel user);

        Task<UserViewModel> UpdateUserAsync(UserViewModel user);

        Task<UserViewModel> RemoveUserAsync(UserViewModel user);

        Task<bool> ExistsUserAsync(int userId);

        Task<bool> ChangePasswordAsync(ChangePasswordViewModel model);



        Task<RoleViewModel> GetRoleAsync(int roleId);

        Task<RolesViewModel> GetRolesAsync(string search, int page = 1, int pageSize = 10);

        Task<List<RoleViewModel>> GetRolesAsync();

        Task<RoleViewModel> AddRoleAsync(RoleViewModel role);

        Task<RoleViewModel> UpdateRoleAsync(RoleViewModel role);

        Task<RoleViewModel> RemoveRoleAsync(RoleViewModel role);

        Task<bool> ExistsRoleAsync(int roleId);





        Task<UserRolesViewModel> GetUserRolesAsync(int userId, int page = 1, int pageSize = 10);

        Task<UserRolesViewModel> AddUserRoleAsync(UserRolesViewModel roles);

        Task<bool> RemoveUserRoleAsync(int userId, int roleId);



        Task<UserClaimsViewModel> GetUserClaimAsync(int userId, int claimId);

        Task<UserClaimsViewModel> GetUserClaimsAsync(int userId, int page = 1, int pageSize = 10);

        Task<UserClaimsViewModel> AddUserClaimAsync(UserClaimsViewModel claims);

        Task<UserClaimsViewModel> UpdateUserClaimAsync(UserClaimsViewModel claims);

        Task<bool> RemoveUserClaimAsync(int userId, int claimId);



        Task<UserProviderViewModel> GetUserProviderAsync(int userId, string providerKey);

        Task<UserProvidersViewModel> GetUserProvidersAsync(int userId);

        Task<bool> RemoveUserProviderAsync(UserProviderViewModel provider);



        Task<RoleClaimsViewModel> GetUserRoleClaimsAsync(int userId, string claimSearch, int page = 1, int pageSize = 10);


        Task<RoleClaimsViewModel> GetRoleClaimAsync(int roleId, int claimId);

        Task<RoleClaimsViewModel> GetRoleClaimsAsync(int roleId, int page = 1, int pageSize = 10);

        Task<RoleClaimsViewModel> AddRoleClaimAsync(RoleClaimsViewModel claims);

        Task<RoleClaimsViewModel> UpdateRoleClaimAsync(RoleClaimsViewModel claims);

        Task<RoleClaimsViewModel> RemoveRoleClaimAsync(RoleClaimsViewModel claim);



        Task<UsersViewModel> GetUsersInRoleAsync(int roleId, string search, int page = 1, int pageSize = 10);


        List<SelectItemViewModel> GetSexTypes();


        UserViewModel BuildUserViewModel(UserViewModel model);
        Task<UserRolesViewModel> BuildUserRolesViewModel(int id, int? page);

        UserClaimsViewModel BuildUserClaimsViewModel(UserClaimsViewModel model);

        RegisterViewModel BuildRegisterViewModel();
    }
}
