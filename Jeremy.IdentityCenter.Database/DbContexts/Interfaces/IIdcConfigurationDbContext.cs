using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Jeremy.IdentityCenter.Database.DbContexts.Interfaces
{
    public interface IIdcConfigurationDbContext : IConfigurationDbContext
    {
        /*
         * 以下表在 ConfigurationDbContext 内部已经创建，为了外部使用，特意添加。
         */
        DbSet<ApiResourceSecret> ApiSecrets { get; set; }

        DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }

        DbSet<IdentityResourceClaim> IdentityClaims { get; set; }

        DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }

        DbSet<ClientGrantType> ClientGrantTypes { get; set; }

        DbSet<ClientScope> ClientScopes { get; set; }

        DbSet<ClientSecret> ClientSecrets { get; set; }

        DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }

        DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }

        DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }

        DbSet<ClientClaim> ClientClaims { get; set; }

        DbSet<ClientProperty> ClientProperties { get; set; }

        DbSet<IdentityResourceProperty> IdentityResourceProperties { get; set; }

        DbSet<ApiResourceProperty> ApiResourceProperties { get; set; }

        DbSet<ApiScopeProperty> ApiScopeProperties { get; set; }

        DbSet<ApiResourceScope> ApiResourceScopes { get; set; }
    }
}
