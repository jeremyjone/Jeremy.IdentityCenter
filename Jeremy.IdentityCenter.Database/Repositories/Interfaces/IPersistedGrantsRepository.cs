using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories.Interfaces;
using Jeremy.IdentityCenter.Database.Entities;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Repositories.Interfaces
{
    public interface IPersistedGrantsRepository : IBaseRepository<PersistedGrant>
    {
        Task<PageList<PersistedGrantData>> GetPersistedGrantsByUsersAsync(string search, int page = 1, int pageSize = 10);

        Task<PageList<PersistedGrant>> GetPersistedGrantsByUserAsync(string id, int page = 1, int pageSize = 10);

        Task<PersistedGrant> GetPersistedGrantAsync(string key);

        Task<bool> DeletePersistedGrantAsync(string key);

        Task<bool> DeletePersistedGrantsAsync(string userId);

        Task<bool> ExistsPersistedGrantsAsync(string id);

        Task<bool> ExistsPersistedGrantAsync(string key);
    }
}
