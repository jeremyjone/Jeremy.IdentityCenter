using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories.Interfaces;
using Jeremy.IdentityCenter.Database.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Repositories.Interfaces
{
    public interface IRoleRepository : IRepository
    {
        /// <summary>
        /// 获取指定 Id 的角色内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IdcIdentityRole> GetAsync(int id);

        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <returns></returns>
        Task<List<IdcIdentityRole>> GetRangeAsync();

        /// <summary>
        /// 获取符合条件的角色集合
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageList<IdcIdentityRole>> GetRangeAsync(string search, int page = 1, int pageSize = 10);

        /// <summary>
        /// 删除指定角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(IdcIdentityRole role);

        /// <summary>
        /// 添加一个角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<bool> AddAsync(IdcIdentityRole role);

        /// <summary>
        /// 更新指定角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(IdcIdentityRole role);

    }
}
