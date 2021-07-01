using Jeremy.IdentityCenter.Database.Constants;
using Jeremy.IdentityCenter.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jeremy.IdentityCenter.Database.DbContexts
{
    public class IdcIdentityDbContext : IdentityDbContext<IdcIdentityUser, IdcIdentityRole, int, IdentityUserClaim<int>,
        IdcIdentityUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public IdcIdentityDbContext(DbContextOptions<IdcIdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ConfiguringDbContext(builder);
        }

        private static void ConfiguringDbContext(ModelBuilder builder)
        {
            builder.Entity<IdcIdentityUser>().ToTable(TableName.IdcUsers);
            builder.Entity<IdcIdentityRole>().ToTable(TableName.IdcRoles);
            builder.Entity<IdentityUserClaim<int>>().ToTable(TableName.IdcUserClaims);
            builder.Entity<IdentityUserLogin<int>>().ToTable(TableName.IdcUserLogins);
            builder.Entity<IdentityRoleClaim<int>>().ToTable(TableName.IdcRoleClaims);
            builder.Entity<IdentityUserToken<int>>().ToTable(TableName.IdcUserTokens);
            builder.Entity<IdcIdentityUserRole>().ToTable(TableName.IdcUserRoles);
        }
    }
}