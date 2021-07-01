using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories.Interfaces;
using Jeremy.IdentityCenter.Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Repositories.Interfaces
{
    public interface IIdentityRepository<TDbContext, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : IRepository
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
        /// <summary>
        /// 查找符合角色的用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="expression">对用户和角色信息进行自定义筛选</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<TUser>> GetUsersInRoleAsync(string roleId, Expression<Func<IdentityUserAndUserRole<TUser, TUserRole>, bool>> expression = null, int page = 1, int pageSize = 10);

        /// <summary>
        /// 查找符合角色的用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="search">查找范围：用户名以及用户邮箱</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<TUser>> GetUsersInRoleAsync(string roleId, string search, int page = 1, int pageSize = 10);

        /// <summary>
        /// 查找用户所含的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<TRole>> GetRolesInUserAsync(string userId, int page = 1, int pageSize = 10);

        /// <summary>
        /// 添加角色到用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<bool> AddRoleToUserAsync(string userId, string roleId);

        /// <summary>
        /// 移除用户的一个角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<bool> DeleteRoleFromUserAsync(string userId, string roleId);




        /// <summary>
        /// 查找符合声明的用户
        /// </summary>
        /// <param name="claimType"></param>
        /// <param name="claimValue"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<TUser>> GetUsersInClaimAsync(string claimType, string claimValue, int page = 1, int pageSize = 10);

        /// <summary>
        /// 查找用户所含的指定声明
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="claimId"></param>
        /// <returns></returns>
        Task<TUserClaim> GetClaimInUserAsync(string userId, int claimId);

        /// <summary>
        /// 查找用户所含的声明
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<TUserClaim>> GetClaimsInUserAsync(string userId, int page = 1, int pageSize = 10);

        /// <summary>
        /// 添加声明到用户
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        Task<bool> AddClaimToUserAsync(TUserClaim claim);

        /// <summary>
        /// 更新用户的声明
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        Task<bool> UpdateClaimToUserAsync(TUserClaim claim);

        /// <summary>
        /// 删除用户的一个声明
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        Task<bool> DeleteClaimFromUserAsync(TUserClaim claim);




        /// <summary>
        /// 获取指定的角色声明
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="claimId"></param>
        /// <returns></returns>
        Task<TRoleClaim> GetRoleClaimAsync(string roleId, int claimId);

        /// <summary>
        /// 获取角色声明集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<TRoleClaim>> GetRoleClaimsAsync(string roleId, int page = 1, int pageSize = 10);

        /// <summary>
        /// 查询指定用户所含的角色声明
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="claimType"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<TRoleClaim>> GetRoleClaimsInUserAsync(string userId, string claimType, int page = 1,
            int pageSize = 10);

        /// <summary>
        /// 添加一个角色声明
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        Task<bool> AddRoleClaimAsync(TRoleClaim claim);

        /// <summary>
        /// 更新一个角色声明
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        Task<bool> UpdateRoleClaimAsync(TRoleClaim claim);

        /// <summary>
        /// 删除一个角色声明
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        Task<bool> DeleteRoleClaimAsync(TRoleClaim claim);
    }
}
