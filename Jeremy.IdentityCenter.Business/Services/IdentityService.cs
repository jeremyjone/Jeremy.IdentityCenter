using Jeremy.IdentityCenter.Business.Common.Models;
using Jeremy.IdentityCenter.Business.Mappers.Extension;
using Jeremy.IdentityCenter.Business.Models;
using Jeremy.IdentityCenter.Business.Models.Account;
using Jeremy.IdentityCenter.Business.Models.Identity;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Jeremy.IdentityCenter.Database.DbContexts;
using Jeremy.IdentityCenter.Database.Entities;
using Jeremy.IdentityCenter.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Services
{
    public class IdentityService :
        BaseService<
            IIdentityRepository<IdcIdentityDbContext, IdcIdentityUser, IdcIdentityRole, int, IdentityUserClaim<int>,
                IdcIdentityUserRole, IdentityUserLogin<int>,
                IdentityRoleClaim<int>, IdentityUserToken<int>>, IIdentityService>, IIdentityService
    {
        public IUserRepository UserRepository { get; }
        public IRoleRepository RoleRepository { get; }

        public IdentityService(ILogger<IdentityService> logger, IUserRepository userRepository,
            IRoleRepository roleRepository,
            IIdentityRepository<IdcIdentityDbContext, IdcIdentityUser, IdcIdentityRole, int, IdentityUserClaim<int>,
                IdcIdentityUserRole, IdentityUserLogin<int>,
                IdentityRoleClaim<int>, IdentityUserToken<int>> identityRepository) : base(identityRepository, logger)
        {
            UserRepository = userRepository;
            RoleRepository = roleRepository;
        }

        public async Task<UserViewModel> GetUserAsync(int userId)
        {
            var user = await UserRepository.GetAsync(userId);
            if (user == null) throw new NullResultException($"Invalid user id - [{userId}].");

            return user.ToViewModel();
        }

        public async Task<UsersViewModel> GetUsersAsync(string search, int page = 1, int pageSize = 10)
        {
            var lists = await UserRepository.GetRangeAsync(search, page, pageSize);
            return lists.ToViewModel();
        }

        public async Task<UserViewModel> AddUserAsync(UserViewModel user)
        {
            var entity = user.ToEntity();
            var res = await UserRepository.AddAsync(entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<UserViewModel> UpdateUserAsync(UserViewModel user)
        {
            var entity = user.ToEntity();

            // 映射 password hash，保持密码的 hash 值不变
            entity.PasswordHash = (await UserRepository.GetAsync(entity.Id)).PasswordHash;

            // 更新
            var res = await UserRepository.UpdateAsync(entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<UserViewModel> RemoveUserAsync(UserViewModel user)
        {
            return await UserRepository.DeleteAsync(user.Id) ? user : null;
        }

        public async Task<bool> ExistsUserAsync(int userId)
        {
            return await UserRepository.GetAsync(userId) != null;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            if (!await ExistsUserAsync(model.UserId)) throw new NullResultException($"Invalid user id - [{model.UserId}].");
            return await UserRepository.ChangePasswordAsync(model.UserId, model.Password);
        }

        public async Task<RoleViewModel> GetRoleAsync(int roleId)
        {
            var role = await RoleRepository.GetAsync(roleId);
            if (role == null) throw new NullResultException($"Invalid role id - [{roleId}].");

            return role.ToViewModel();
        }

        public async Task<RolesViewModel> GetRolesAsync(string search, int page = 1, int pageSize = 10)
        {
            var roles = await RoleRepository.GetRangeAsync(search, page, pageSize);
            return roles.ToViewModel<RolesViewModel>();
        }

        public async Task<List<RoleViewModel>> GetRolesAsync()
        {
            var roles = await RoleRepository.GetRangeAsync();
            return roles.ToViewModel();
        }

        public async Task<RoleViewModel> AddRoleAsync(RoleViewModel role)
        {
            var entity = role.ToEntity();
            var res = await RoleRepository.AddAsync(entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<RoleViewModel> UpdateRoleAsync(RoleViewModel role)
        {
            var entity = role.ToEntity();

            var res = await RoleRepository.UpdateAsync(entity);
            return res ? entity.ToViewModel() : null;
        }

        public async Task<RoleViewModel> RemoveRoleAsync(RoleViewModel role)
        {
            var entity = role.ToEntity();
            return await RoleRepository.DeleteAsync(entity) ? role : null;
        }

        public async Task<bool> ExistsRoleAsync(int roleId)
        {
            return await RoleRepository.GetAsync(roleId) != null;
        }

        public async Task<UserRolesViewModel> GetUserRolesAsync(int userId, int page = 1, int pageSize = 10)
        {
            var user = await UserRepository.GetAsync(userId);
            if (user == null) throw new NullResultException($"Invalid user id - [{userId}].");

            var userRoles = await Repository.GetRolesInUserAsync(userId.ToString(), page, pageSize);
            var models = userRoles.ToViewModel<UserRolesViewModel>();
            models.UserName = user.UserName;
            return models;
        }

        public async Task<UserRolesViewModel> AddUserRoleAsync(UserRolesViewModel roles)
        {
            var res = await Repository.AddRoleToUserAsync(roles.UserId.ToString(), roles.RoleId.ToString());
            return res ? await BuildUserRolesViewModel(roles.UserId, 1) : null;
        }

        public async Task<bool> RemoveUserRoleAsync(int userId, int roleId)
        {
            return await Repository.DeleteRoleFromUserAsync(userId.ToString(), roleId.ToString());
        }

        public async Task<UserClaimsViewModel> GetUserClaimAsync(int userId, int claimId)
        {
            if (!await ExistsUserAsync(userId)) throw new NullResultException($"Invalid user id - [{userId}].");

            var claim = await Repository.GetClaimInUserAsync(userId.ToString(), claimId);
            if (claim == null) throw new NullResultException($"Invalid claim id - [{claimId}].");

            return claim.ToViewModel();
        }

        public async Task<UserClaimsViewModel> GetUserClaimsAsync(int userId, int page = 1, int pageSize = 10)
        {
            var user = await UserRepository.GetAsync(userId);
            if (user == null) throw new NullResultException($"Invalid user id - [{userId}].");

            var claims = await Repository.GetClaimsInUserAsync(userId.ToString(), page, pageSize);
            var models = claims.ToViewModel<UserClaimsViewModel>();

            models.UserName = user.UserName;
            return models;
        }

        public async Task<UserClaimsViewModel> AddUserClaimAsync(UserClaimsViewModel claims)
        {
            var entity = claims.ToEntity();
            var res = await Repository.AddClaimToUserAsync(entity);

            return res ? entity.ToViewModel() : null;
        }

        public async Task<UserClaimsViewModel> UpdateUserClaimAsync(UserClaimsViewModel claims)
        {
            var entity = claims.ToEntity();
            var res = await Repository.UpdateClaimToUserAsync(entity);

            return res ? entity.ToViewModel() : null;
        }

        public async Task<bool> RemoveUserClaimAsync(int userId, int claimId)
        {
            var claim = await Repository.GetClaimInUserAsync(userId.ToString(), claimId);
            return await Repository.DeleteClaimFromUserAsync(claim);
        }

        public async Task<UserProviderViewModel> GetUserProviderAsync(int userId, string providerKey)
        {
            var user = await UserRepository.GetAsync(userId);
            if (user == null) throw new NullResultException($"Invalid user id - [{userId}].");

            var login = await UserRepository.GetUserProviderAsync(userId, providerKey);
            if (login == null) throw new NullResultException($"Invalid provider key - [{providerKey}].");

            var model = login.ToViewModel();
            model.UserName = user.UserName;
            return model;
        }

        public async Task<UserProvidersViewModel> GetUserProvidersAsync(int userId)
        {
            var user = await UserRepository.GetAsync(userId);
            if (user == null) throw new NullResultException($"Invalid user id - [{userId}].");

            var logins = await UserRepository.GetUserProvidersAsync(userId);
            var models = logins.ToViewModel();
            models.UserId = userId;
            models.UserName = user.UserName;
            return models;
        }

        public async Task<bool> RemoveUserProviderAsync(UserProviderViewModel provider)
        {
            return await UserRepository.DeleteUserProviderAsync(provider.UserId, provider.ProviderKey,
                provider.LoginProvider);
        }

        public async Task<RoleClaimsViewModel> GetUserRoleClaimsAsync(int userId, string claimSearch, int page = 1,
            int pageSize = 10)
        {
            var user = await UserRepository.GetAsync(userId);
            if (user == null) throw new NullResultException($"Invalid user id - [{userId}].");

            var claims = await Repository.GetRoleClaimsInUserAsync(userId.ToString(), claimSearch, page, pageSize);
            return claims.ToViewModel();
        }

        public async Task<RoleClaimsViewModel> GetRoleClaimAsync(int roleId, int claimId)
        {
            var role = await RoleRepository.GetAsync(roleId);
            if (role == null) throw new NullResultException($"Invalid role id - [{roleId}].");

            var roleClaim = await Repository.GetRoleClaimAsync(roleId.ToString(), claimId);
            var model = roleClaim.ToViewModel();
            model.RoleName = role.Name;
            return model;
        }

        public async Task<RoleClaimsViewModel> GetRoleClaimsAsync(int roleId, int page = 1, int pageSize = 10)
        {
            var role = await RoleRepository.GetAsync(roleId);
            if (role == null) throw new NullResultException($"Invalid role id - [{roleId}].");

            var roleClaims = await Repository.GetRoleClaimsAsync(roleId.ToString(), page, pageSize);
            var model = roleClaims.ToViewModel();
            model.RoleName = role.Name;
            return model;
        }

        public async Task<RoleClaimsViewModel> AddRoleClaimAsync(RoleClaimsViewModel claims)
        {
            var entity = claims.ToEntity();
            return await Repository.AddRoleClaimAsync(entity) ? entity.ToViewModel() : null;
        }

        public async Task<RoleClaimsViewModel> UpdateRoleClaimAsync(RoleClaimsViewModel claims)
        {
            var entity = claims.ToEntity();
            return await Repository.UpdateRoleClaimAsync(entity) ? entity.ToViewModel() : null;
        }

        public async Task<RoleClaimsViewModel> RemoveRoleClaimAsync(RoleClaimsViewModel claim)
        {
            return await Repository.DeleteRoleClaimAsync(claim.ToEntity()) ? claim : null;
        }

        public async Task<UsersViewModel> GetUsersInRoleAsync(int roleId, string search, int page = 1, int pageSize = 10)
        {
            var role = await RoleRepository.GetAsync(roleId);
            if (role == null) throw new NullResultException($"Invalid role id - [{roleId}].");

            // 自定义查询内容
            //var users = await Repository.GetUsersInRoleAsync(roleId.ToString(),
            //    x => x.User.UserName.Contains(search) || x.User.NickName.Contains(search) ||
            //         x.User.Email.Contains(search), page, pageSize);

            // 默认查询，仅包含用户名和邮箱
            var users = await Repository.GetUsersInRoleAsync(roleId.ToString(), search, page, pageSize);
            return users.ToViewModel();
        }

        public List<SelectItemViewModel> GetSexTypes()
        {
            return UserRepository.GetSexTypes().ToViewModel();
        }

        public UserViewModel BuildUserViewModel(UserViewModel model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<UserRolesViewModel> BuildUserRolesViewModel(int id, int? page)
        {
            var roles = await RoleRepository.GetRangeAsync();
            var userRoles = await GetUserRolesAsync(id, page ?? 1);
            userRoles.UserId = id;
            userRoles.RolesList = roles.Select(x => new SelectItemViewModel(x.Id.ToString(), x.Name)).ToList();
            return userRoles;
        }

        public UserClaimsViewModel BuildUserClaimsViewModel(UserClaimsViewModel model)
        {
            throw new System.NotImplementedException();
        }

        public RegisterViewModel BuildRegisterViewModel()
        {
            return new RegisterViewModel { SexList = GetSexTypes() };
        }
    }
}