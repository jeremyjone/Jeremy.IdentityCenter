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
    public class ApiScopeRepository<TContext> :
        BaseRepository<ApiScope, TContext, IApiScopeRepository>, IApiScopeRepository
        where TContext : DbContext, IIdcConfigurationDbContext
    {
        public ApiScopeRepository(TContext db, ILogger<IApiScopeRepository> logger) : base(db, logger)
        {
        }

        public async Task<ApiScope> GetAsync(int id)
        {
            return await Db.ApiScopes.Include(x => x.UserClaims)
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public override async Task<PageList<ApiScope>> GetRangeAsync(Expression<Func<ApiScope, bool>> expression, int page, int pageSize = 10)
        {
            return await GetRangeAsync(expression, x => x.Name, page, pageSize);
        }

        public async Task<ApiScopeProperty> GetApiScopePropertyAsync(int propertyId)
        {
            return await GetAccessoryAsync<ApiScopeProperty>(x => x.Id == propertyId, x => x.Scope);
        }

        public async Task<PageList<ApiScopeProperty>> GetApiScopePropertiesAsync(int scopeId, int page, int pageSize)
        {
            return await GetAccessoryRangeAsync<ApiScopeProperty, int>(x => x.Scope.Id == scopeId,
                x => x.Id, page, pageSize);
        }

        public async Task<bool> AddApiScopePropertyAsync(int scopeId, ApiScopeProperty entity)
        {
            entity.Scope = await GetAsync(x => x.Id == scopeId);
            return await AddAccessoryAsync(entity);
        }

        public async Task<bool> DeleteApiScopePropertyAsync(ApiScopeProperty property)
        {
            return await DeleteAccessoryAsync<ApiScopeProperty>(x => x.Id == property.Id);
        }

        public async Task<bool> ExistApiScopeAsync(ApiScope scope)
        {
            return null != await Db.ApiScopes.FirstOrDefaultAsync(x =>
                x.Name == scope.Name && (scope.Id == 0 || scope.Id != x.Id));
        }

        public async Task<bool> ExistApiScopePropertyAsync(ApiScopeProperty property)
        {
            return null != await Db.ApiScopeProperties.FirstOrDefaultAsync(x =>
                x.Key == property.Key && x.Scope.Id == property.ScopeId);
        }
    }
}