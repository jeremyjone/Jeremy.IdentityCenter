using Jeremy.IdentityCenter.Business.Common.Models;
using Jeremy.IdentityCenter.Business.Mappers.Extension;
using Jeremy.IdentityCenter.Business.Models.PersistedGrants;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Jeremy.IdentityCenter.Database.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Services
{
    public class PersistedGrantService : BaseService<IPersistedGrantsRepository, IPersistedGrantService>, IPersistedGrantService
    {
        public PersistedGrantService(IPersistedGrantsRepository repository, ILogger<IPersistedGrantService> logger) : base(repository, logger)
        {
        }

        public async Task<PersistedGrantsViewModel> GetPersistedGrantsByUsersAsync(string search, int page = 1, int pageSize = 10)
        {
            return (await Repository.GetPersistedGrantsByUsersAsync(search, page, pageSize)).ToViewModel();
        }

        public async Task<PersistedGrantsViewModel> GetPersistedGrantsByUserAsync(string subjectId, int page = 1, int pageSize = 10)
        {
            if (!await Repository.ExistsPersistedGrantsAsync(subjectId)) throw new NullResultException($"Invalid subject id - [{subjectId}].");
            return (await Repository.GetPersistedGrantsByUserAsync(subjectId, page, pageSize)).ToViewModel();
        }

        public async Task<PersistedGrantViewModel> GetPersistedGrantAsync(string key)
        {
            if (!await Repository.ExistsPersistedGrantAsync(key)) throw new NullResultException($"Invalid subject key - [{key}].");
            return (await Repository.GetPersistedGrantAsync(key)).ToViewModel();
        }

        public async Task<bool> DeletePersistedGrantAsync(string key)
        {
            if (!await Repository.ExistsPersistedGrantAsync(key)) throw new NullResultException($"Invalid subject key - [{key}].");
            return await Repository.DeletePersistedGrantAsync(key);
        }

        public async Task<bool> DeletePersistedGrantsAsync(string userId)
        {
            if (!await Repository.ExistsPersistedGrantsAsync(userId)) throw new NullResultException($"Invalid user id - [{userId}].");
            return await Repository.DeletePersistedGrantsAsync(userId);
        }
    }
}