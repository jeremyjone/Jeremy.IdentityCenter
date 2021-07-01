using IdentityServer4.EntityFramework.Entities;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories;
using Jeremy.IdentityCenter.Database.DbContexts.Interfaces;
using Jeremy.IdentityCenter.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Repositories
{
    public class IdentityResourceRepository<TContext> :
        BaseRepository<IdentityResource, TContext, IIdentityResourceRepository>, IIdentityResourceRepository
        where TContext : DbContext, IIdcConfigurationDbContext
    {
        public IdentityResourceRepository(TContext db, ILogger<IIdentityResourceRepository> logger) : base(db, logger)
        {
        }

        public async Task<IdentityResource> GetAsync(int id)
        {
            return await Db.IdentityResources.Include(x => x.UserClaims)
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public override async Task<PageList<IdentityResource>> GetRangeAsync(Expression<Func<IdentityResource, bool>> expression, int page, int pageSize = 10)
        {
            return await GetRangeAsync(expression, x => x.Name, page, pageSize);
        }

        public async Task<IdentityResourceProperty> GetIdentityResourcePropertyAsync(int propertyId)
        {
            return await GetAccessoryAsync<IdentityResourceProperty>(x => x.Id == propertyId, x => x.IdentityResource);
        }

        public async Task<PageList<IdentityResourceProperty>> GetIdentityResourcePropertiesAsync(int resourceId, int page, int pageSize)
        {
            return await GetAccessoryRangeAsync<IdentityResourceProperty, int>(x => x.IdentityResource.Id == resourceId,
                x => x.Id, page, pageSize);
        }

        public async Task<bool> AddIdentityResourcePropertyAsync(int resourceId, IdentityResourceProperty entity)
        {
            entity.IdentityResource = await GetAsync(x => x.Id == resourceId);
            return await AddAccessoryAsync(entity);
        }

        public async Task<bool> DeleteIdentityResourcePropertyAsync(IdentityResourceProperty property)
        {
            return await DeleteAccessoryAsync<IdentityResourceProperty>(x => x.Id == property.Id);
        }

        public async Task<bool> ExistIdentityResourceAsync(IdentityResource resource)
        {
            return null != await Db.IdentityResources.FirstOrDefaultAsync(x =>
                x.Name == resource.Name && (resource.Id == 0 || resource.Id != x.Id));
        }

        public async Task<bool> ExistIdentityResourcePropertyAsync(IdentityResourceProperty property)
        {
            return null != await Db.IdentityResourceProperties.FirstOrDefaultAsync(x =>
                x.Key == property.Key && x.IdentityResource.Id == property.IdentityResourceId);
        }
    }
}