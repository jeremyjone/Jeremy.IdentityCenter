using Jeremy.IdentityCenter.Business.Models.PersistedGrants;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Services.Interfaces
{
    public interface IPersistedGrantService : IBaseService
    {
        Task<PersistedGrantsViewModel> GetPersistedGrantsByUsersAsync(string search, int page = 1, int pageSize = 10);
        Task<PersistedGrantsViewModel> GetPersistedGrantsByUserAsync(string subjectId, int page = 1, int pageSize = 10);
        Task<PersistedGrantViewModel> GetPersistedGrantAsync(string key);
        Task<bool> DeletePersistedGrantAsync(string key);
        Task<bool> DeletePersistedGrantsAsync(string userId);
    }
}
