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
    public class ApiResourceRepository<TContext> :
        BaseRepository<ApiResource, TContext, IApiResourceRepository>, IApiResourceRepository
        where TContext : DbContext, IIdcConfigurationDbContext
    {
        public ApiResourceRepository(TContext db, ILogger<IApiResourceRepository> logger) : base(db, logger)
        {

        }

        public async Task<ApiResource> GetAsync(int id)
        {
            return await Db.ApiResources.Include(x => x.UserClaims)
                .Include(x => x.Scopes)
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public override async Task<PageList<ApiResource>> GetRangeAsync(Expression<Func<ApiResource, bool>> expression, int page, int pageSize = 10)
        {
            return await GetRangeAsync(expression, x => x.Name, page, pageSize);
        }

        public async Task<ApiResourceProperty> GetApiResourcePropertyAsync(int propertyId)
        {
            return await GetAccessoryAsync<ApiResourceProperty>(x => x.Id == propertyId, x => x.ApiResource);
        }

        public async Task<PageList<ApiResourceProperty>> GetApiResourcePropertiesAsync(int resourceId, int page = 1, int pageSize = 10)
        {
            return await GetAccessoryRangeAsync<ApiResourceProperty, int>(x => x.ApiResource.Id == resourceId,
                x => x.Id, page, pageSize);
        }

        public async Task<bool> AddApiResourcePropertyAsync(int resourceId, ApiResourceProperty entity)
        {
            entity.ApiResource = await GetAsync(x => x.Id == resourceId);
            return await AddAccessoryAsync(entity);
        }

        public async Task<bool> DeleteApiResourcePropertyAsync(ApiResourceProperty property)
        {
            return await DeleteAccessoryAsync<ApiResourceProperty>(x => x.Id == property.Id);
        }

        public async Task<ApiResourceSecret> GetApiResourceSecretAsync(int secretId)
        {
            return await GetAccessoryAsync<ApiResourceSecret>(x => x.Id == secretId, x => x.ApiResource);
        }

        public async Task<PageList<ApiResourceSecret>> GetApiResourceSecretsAsync(int resourceId, int page = 1, int pageSize = 10)
        {
            return await GetAccessoryRangeAsync<ApiResourceSecret, int>(x => x.ApiResource.Id == resourceId,
                x => x.Id, page, pageSize);
        }

        public async Task<bool> AddApiResourceSecretAsync(int resourceId, ApiResourceSecret entity)
        {
            entity.ApiResource = await GetAsync(x => x.Id == resourceId);
            return await AddAccessoryAsync(entity);
        }

        public async Task<bool> DeleteApiResourceSecretAsync(ApiResourceSecret secret)
        {
            return await DeleteAccessoryAsync<ApiResourceSecret>(x => x.Id == secret.Id);
        }

        public async Task<bool> ExistApiResourceAsync(ApiResource resource)
        {
            return null != await Db.ApiResources.FirstOrDefaultAsync(x =>
                x.Name == resource.Name && (resource.Id == 0 || resource.Id != x.Id));
        }

        public async Task<bool> ExistApiResourcePropertyAsync(ApiResourceProperty property)
        {
            return null != await Db.ApiResourceProperties.FirstOrDefaultAsync(x =>
                x.Key == property.Key && x.ApiResource.Id == property.ApiResourceId);
        }
    }
}