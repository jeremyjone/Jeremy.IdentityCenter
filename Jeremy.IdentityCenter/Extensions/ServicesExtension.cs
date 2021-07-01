using Jeremy.IdentityCenter.Business.Services;
using Jeremy.IdentityCenter.Business.Services.Interfaces;
using Jeremy.IdentityCenter.Database.DbContexts;
using Jeremy.IdentityCenter.Database.Entities;
using Jeremy.IdentityCenter.Database.Repositories;
using Jeremy.IdentityCenter.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Jeremy.IdentityCenter.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddIdcServices(this IServiceCollection services)
        {
            #region 添加 Repositories

            services.AddTransient<IClientRepository, ClientRepository<IdcConfigurationDbContext>>();
            services.AddTransient<IIdentityResourceRepository, IdentityResourceRepository<IdcConfigurationDbContext>>();
            services.AddTransient<IApiScopeRepository, ApiScopeRepository<IdcConfigurationDbContext>>();
            services.AddTransient<IApiResourceRepository, ApiResourceRepository<IdcConfigurationDbContext>>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IPersistedGrantsRepository, PersistedGrantsRepository>();
            services
                .AddTransient<IIdentityRepository<IdcIdentityDbContext, IdcIdentityUser, IdcIdentityRole, int,
                    IdentityUserClaim<int>, IdcIdentityUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
                    IdentityUserToken<int>>, IdentityRepository<IdcIdentityDbContext, IdcIdentityUser, IdcIdentityRole, int,
                    IdentityUserClaim<int>, IdcIdentityUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
                    IdentityUserToken<int>>>();

            #endregion


            #region 添加 Services

            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IIdentityResourceService, IdentityResourceService>();
            services.AddTransient<IApiScopeService, ApiScopeService>();
            services.AddTransient<IApiResourceService, ApiResourceService>();
            services.AddTransient<IPersistedGrantService, PersistedGrantService>();
            services.AddTransient<IIdentityService, IdentityService>();

            #endregion


            return services;
        }
    }
}
