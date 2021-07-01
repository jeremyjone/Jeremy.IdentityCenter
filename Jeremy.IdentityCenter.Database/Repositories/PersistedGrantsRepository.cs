using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Database.Common.Extensions;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories;
using Jeremy.IdentityCenter.Database.DbContexts;
using Jeremy.IdentityCenter.Database.Entities;
using Jeremy.IdentityCenter.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Repositories
{
    public class PersistedGrantsRepository :
        BaseRepository<PersistedGrant, IdcPersistedGrantDbContext, IPersistedGrantsRepository>,
        IPersistedGrantsRepository
    {
        public IdcIdentityDbContext IdentityDbContext { get; }

        public PersistedGrantsRepository(IdcPersistedGrantDbContext db, ILogger<IPersistedGrantsRepository> logger, IdcIdentityDbContext identityDbContext) :
            base(db, logger)
        {
            IdentityDbContext = identityDbContext;
        }

        public async Task<PageList<PersistedGrantData>> GetPersistedGrantsByUsersAsync(string search, int page = 1, int pageSize = 10)
        {
            var grants = (await Db.PersistedGrants.ToListAsync())
                .GroupJoin(IdentityDbContext.Users.ToList(),
                    pg => pg.SubjectId,
                    u => u.Id.ToString(),
                    (pg, users) => new { pg, users })
                .SelectMany(x => x.users.DefaultIfEmpty(),
                    (x, identity) => new PersistedGrantData
                    {
                        SubjectId = x.pg.SubjectId,
                        SubjectName = identity == null ? string.Empty : identity.UserName
                    })
                .GroupBy(x => x.SubjectId)
                .Select(x => x.First());

            var enumerable = grants.Where(x =>
                    string.IsNullOrWhiteSpace(search) ||
                    x.SubjectId.Contains(search) ||
                    x.SubjectName.Contains(search))
                .ToList();

            return enumerable
                .AsQueryable()
                .PageBy(x => x.SubjectId, page, pageSize)
                .ToPageList(enumerable.Count(), pageSize);
        }

        public override async Task<PageList<PersistedGrant>> GetRangeAsync(
            Expression<Func<PersistedGrant, bool>> expression, int page, int pageSize = 10)
        {
            var data = await Db.PersistedGrants.Where(expression).PageBy(x => x.SubjectId, page, pageSize)
                .ToListAsync();

            return data.ToPageList(data.Count, pageSize);
        }

        public async Task<PageList<PersistedGrant>> GetPersistedGrantsByUserAsync(string id, int page = 1,
            int pageSize = 10)
        {
            return await GetRangeAsync(x => x.SubjectId == id, page, pageSize);
        }

        public async Task<PersistedGrant> GetPersistedGrantAsync(string key)
        {
            return await Db.PersistedGrants.FirstOrDefaultAsync(x => x.Key == key);
        }

        public async Task<bool> DeletePersistedGrantAsync(string key)
        {
            var grant = await Db.PersistedGrants.FirstOrDefaultAsync(x => x.Key == key);
            Db.PersistedGrants.Remove(grant);
            return await SaveAsync();
        }

        public async Task<bool> DeletePersistedGrantsAsync(string userId)
        {
            var grants = await Db.PersistedGrants.Where(x => x.SubjectId == userId).ToListAsync();
            Db.PersistedGrants.RemoveRange(grants);
            return await SaveAsync();
        }

        public async Task<bool> ExistsPersistedGrantsAsync(string id)
        {
            return await Db.PersistedGrants.AnyAsync(x => x.SubjectId == id);
        }

        public async Task<bool> ExistsPersistedGrantAsync(string key)
        {
            return await Db.PersistedGrants.AnyAsync(x => x.Key == key);
        }
    }
}