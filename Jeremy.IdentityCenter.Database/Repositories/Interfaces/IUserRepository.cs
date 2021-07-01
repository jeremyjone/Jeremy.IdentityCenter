using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories.Interfaces;
using Jeremy.IdentityCenter.Database.Entities;
using Jeremy.IdentityCenter.Database.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Repositories.Interfaces
{
    public interface IUserRepository : IRepository
    {
        /// <summary>
        /// 获取指定范围的用户信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<IdcIdentityUser>> GetRangeAsync(string search, int page = 1, int pageSize = 10);

        /// <summary>
        /// 获取指定 Id 的用户内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IdcIdentityUser> GetAsync(int id);

        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> AddAsync(IdcIdentityUser user);

        /// <summary>
        /// 更新一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(IdcIdentityUser user);

        /// <summary>
        /// 删除指定用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);



        /// <summary>
        /// 获取用户的提供程序集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<UserLoginInfo>> GetUserProvidersAsync(int id);

        /// <summary>
        /// 获取用户名下指定的提供程序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        Task<IdentityUserLogin<int>> GetUserProviderAsync(int id, string providerKey);

        /// <summary>
        /// 删除用户名下指定的提供程序
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="providerKey"></param>
        /// <param name="loginProvider"></param>
        /// <returns></returns>
        Task<bool> DeleteUserProviderAsync(int userId, string providerKey, string loginProvider);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> ChangePasswordAsync(int userId, string password);

        List<SelectItem> GetSexTypes();
    }
}
