using Jeremy.IdentityCenter.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Jeremy.IdentityCenter.Database.Configuration.SqlServer
{
    public static class DbRegister
    {
        public static void AddSqlServerDbContext(this IServiceCollection services, string connectionString, MigrationAssemblies assemblies)
        {
            var migrationAssembly = typeof(DbRegister).GetTypeInfo().Assembly.GetName().Name;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("数据库配置异常，请检查 appsettings.json");
            }

            services.AddDbContext<IdcIdentityDbContext>(options =>
                options.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(assemblies.IdentityDbMigration ?? migrationAssembly)));

            services.AddDbContext<IdcConfigurationDbContext>(options =>
                options.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(assemblies.ConfigurationDbMigration ?? migrationAssembly)));

            services.AddDbContext<IdcPersistedGrantDbContext>(options =>
                options.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(assemblies.PersistedGrantDbMigration ?? migrationAssembly)));

            services.AddDbContext<IdcDataProtectionDbContext>(options =>
                options.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(assemblies.DataProtectionDbMigration ?? migrationAssembly)));
        }
    }
}
