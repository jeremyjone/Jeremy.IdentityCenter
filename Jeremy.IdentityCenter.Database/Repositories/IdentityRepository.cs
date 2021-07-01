using Jeremy.IdentityCenter.Database.Common.Extensions;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories;
using Jeremy.IdentityCenter.Database.Models;
using Jeremy.IdentityCenter.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Repositories
{
    public class IdentityRepository<TDbContext, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> :
            Repository<TDbContext, IIdentityRepository<TDbContext, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>>,
            IIdentityRepository<TDbContext, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TDbContext : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        public UserManager<TUser> UserManager { get; }
        public RoleManager<TRole> RoleManager { get; }

        public IdentityRepository(TDbContext db,
            ILogger<IIdentityRepository<TDbContext, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>> logger,
            UserManager<TUser> userManager,
            RoleManager<TRole> roleManager) : base(db,
            logger)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public async Task<PageList<TUser>> GetUsersInRoleAsync(string roleId, Expression<Func<IdentityUserAndUserRole<TUser, TUserRole>, bool>> expression = null, int page = 1, int pageSize = 10)
        {
            var users = Db.Users.Join(Db.UserRoles, u => u.Id, ur => ur.UserId,
                    (u, ur) => new IdentityUserAndUserRole<TUser, TUserRole> { User = u, UserRole = ur })
                .Where(x => x.UserRole.RoleId.ToString().Equals(roleId))
                .WhereIf(expression != null, expression)
                .Select(x => x.User);

            return users.PageBy(x => x.Id, page, pageSize).ToPageList(await users.CountAsync(), pageSize);
        }

        public async Task<PageList<TUser>> GetUsersInRoleAsync(string roleId, string search, int page = 1, int pageSize = 10)
        {
            var users = Db.Users.Join(Db.UserRoles, u => u.Id, ur => ur.UserId,
                    (u, ur) => new IdentityUserAndUserRole<TUser, TUserRole> { User = u, UserRole = ur })
                .Where(x => x.UserRole.RoleId.ToString().Equals(roleId))
                .WhereIf(!string.IsNullOrWhiteSpace(search), x =>
                    x.User.UserName.Contains(search) || x.User.Email.Contains(search))
                .Select(x => x.User);

            return users.PageBy(x => x.Id, page, pageSize).ToPageList(await users.CountAsync(), pageSize);
        }

        public async Task<PageList<TRole>> GetRolesInUserAsync(string userId, int page = 1, int pageSize = 10)
        {
            var roles = Db.Roles.Join(Db.UserRoles, r => r.Id, ur => ur.RoleId, (r, ur) => new { r, ur })
                .Where(x => x.ur.UserId.ToString().Equals(userId))
                .Select(x => x.r);

            return roles.PageBy(x => x.Id, page, pageSize).ToPageList(await roles.CountAsync(), pageSize);
        }

        public async Task<bool> AddRoleToUserAsync(string userId, string roleId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            var role = await RoleManager.FindByIdAsync(roleId);
            return (await UserManager.AddToRoleAsync(user, role.Name)).Succeeded;
        }

        public async Task<bool> DeleteRoleFromUserAsync(string userId, string roleId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            var role = await RoleManager.FindByIdAsync(roleId);
            return (await UserManager.RemoveFromRoleAsync(user, role.Name)).Succeeded;
        }

        public async Task<PageList<TUser>> GetUsersInClaimAsync(string claimType, string claimValue, int page = 1,
            int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(claimType))
                throw new ArgumentNullException(claimType, "Claim type cannot be null or empty.");

            var users = Db.Users.Join(Db.UserClaims, u => u.Id, uc => uc.UserId, (u, uc) => new { u, uc })
                .Where(x => x.uc.ClaimType.Equals(claimType))
                .WhereIf(!string.IsNullOrWhiteSpace(claimValue), x => x.uc.ClaimValue.Equals(claimValue))
                .Select(x => x.u).Distinct();

            return users.PageBy(x => x.Id, page, pageSize).ToPageList(await users.CountAsync(), pageSize);
        }

        public async Task<TUserClaim> GetClaimInUserAsync(string userId, int claimId)
        {
            return await Db.UserClaims.FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId) && x.Id == claimId);
        }

        public async Task<PageList<TUserClaim>> GetClaimsInUserAsync(string userId, int page = 1, int pageSize = 10)
        {
            return Db.UserClaims.Where(x => x.UserId.ToString().Equals(userId))
                .PageBy(x => x.Id, page, pageSize)
                .ToPageList(await Db.UserClaims.Where(x => x.UserId.Equals(userId)).CountAsync(), pageSize);
        }

        public async Task<bool> AddClaimToUserAsync(TUserClaim claim)
        {
            var user = await UserManager.FindByIdAsync(claim.UserId.ToString());
            return (await UserManager.AddClaimAsync(user, new Claim(claim.ClaimType, claim.ClaimValue))).Succeeded;
        }

        public async Task<bool> UpdateClaimToUserAsync(TUserClaim claim)
        {
            var user = await UserManager.FindByIdAsync(claim.UserId.ToString());
            var oldClaim = await Db.UserClaims.FirstOrDefaultAsync(x => x.Id == claim.Id);

            // 先删除，后添加
            await UserManager.RemoveClaimAsync(user, new Claim(oldClaim.ClaimType, oldClaim.ClaimValue));
            return (await UserManager.AddClaimAsync(user, new Claim(claim.ClaimType, claim.ClaimValue))).Succeeded;
        }

        public async Task<bool> DeleteClaimFromUserAsync(TUserClaim claim)
        {
            var user = await UserManager.FindByIdAsync(claim.UserId.ToString());
            return (await UserManager.RemoveClaimAsync(user, new Claim(claim.ClaimType, claim.ClaimValue))).Succeeded;
        }

        public async Task<TRoleClaim> GetRoleClaimAsync(string roleId, int claimId)
        {
            return await Db.RoleClaims.FirstOrDefaultAsync(x => x.RoleId.ToString().Equals(roleId) && x.Id == claimId);
        }

        public async Task<PageList<TRoleClaim>> GetRoleClaimsAsync(string roleId, int page = 1, int pageSize = 10)
        {
            return Db.RoleClaims.Where(x => x.RoleId.ToString().Equals(roleId))
                .PageBy(x => x.Id, page, pageSize)
                .ToPageList(await Db.RoleClaims.Where(x => x.RoleId.ToString().Equals(roleId)).CountAsync(), pageSize);
        }

        public async Task<PageList<TRoleClaim>> GetRoleClaimsInUserAsync(string userId, string claimType, int page = 1,
            int pageSize = 10)
        {
            var claims = Db.UserRoles.Where(x => x.UserId.ToString().Equals(userId))
                .Join(Db.RoleClaims.WhereIf(!string.IsNullOrWhiteSpace(claimType), x => x.ClaimType.Contains(claimType)),
                    ur => ur.RoleId, rc => rc.RoleId, (ur, rc) => rc);

            return claims.PageBy(x => x.Id, page, pageSize).ToPageList(await claims.CountAsync(), pageSize);
        }

        public async Task<bool> AddRoleClaimAsync(TRoleClaim claim)
        {
            var role = await RoleManager.FindByIdAsync(claim.RoleId.ToString());
            return (await RoleManager.AddClaimAsync(role, new Claim(claim.ClaimType, claim.ClaimValue))).Succeeded;
        }

        public async Task<bool> UpdateRoleClaimAsync(TRoleClaim claim)
        {
            var role = await RoleManager.FindByIdAsync(claim.RoleId.ToString());
            var oldClaim = await Db.UserClaims.FirstOrDefaultAsync(x => x.Id == claim.Id);

            // 先删除，后添加
            await RoleManager.RemoveClaimAsync(role, new Claim(oldClaim.ClaimType, oldClaim.ClaimValue));
            return (await RoleManager.AddClaimAsync(role, new Claim(claim.ClaimType, claim.ClaimValue))).Succeeded;
        }

        public async Task<bool> DeleteRoleClaimAsync(TRoleClaim claim)
        {
            var role = await RoleManager.FindByIdAsync(claim.RoleId.ToString());
            var oldClaim = await Db.UserClaims.FirstOrDefaultAsync(x => x.Id == claim.Id);

            return (await RoleManager.RemoveClaimAsync(role, new Claim(oldClaim.ClaimType, oldClaim.ClaimValue))).Succeeded;
        }
    }
}