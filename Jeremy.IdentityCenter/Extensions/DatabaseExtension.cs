using Jeremy.IdentityCenter.Database.Configuration;
using Jeremy.IdentityCenter.Database.Configuration.MySql;
using Jeremy.IdentityCenter.Database.Configuration.SqlServer;
using Jeremy.IdentityCenter.Database.MySQL;
using Jeremy.IdentityCenter.Database.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Jeremy.IdentityCenter.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddIdcDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            //var databaseProvider = configuration["DatabaseProvider"];
            //var connectionString = configuration[$"ConnectionStrings:{databaseProvider}"];
            //if (string.IsNullOrWhiteSpace(connectionString))
            //{
            //    throw new Exception("数据库配置异常，请检查 databases.json");
            //}

            //services.AddDbContext<IdcIdentityDbContext>(options =>
            //    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            //services.AddDbContext<IdcConfigurationDbContext>(options =>
            //    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            //services.AddDbContext<IdcPersistedGrantDbContext>(options =>
            //    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            //services.AddDbContext<IdcDataProtectionDbContext>(options =>
            //    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


            #region 多版本数据库分治，多种数据库依次添加即可


            var databaseProvider = new DatabaseProvider();
            configuration.GetSection(nameof(DatabaseProvider)).Bind(databaseProvider);

            var connectionString = configuration[$"ConnectionStrings:{databaseProvider.Type}"];

            switch (databaseProvider.Type)
            {
                case DatabaseTypes.MySql:
                    services.AddMySqlDbContext(connectionString, Assemblies(databaseProvider));
                    break;
                case DatabaseTypes.SqlServer:
                    services.AddSqlServerDbContext(connectionString, Assemblies(databaseProvider));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseProvider), "不支持的数据库，请检查 appsettings.json");
            }

            #endregion



            return services;
        }



        private static MigrationAssemblies Assemblies(DatabaseProvider databaseProvider)
        {
            var assembly = databaseProvider.Type switch
            {
                DatabaseTypes.MySql => typeof(MySqlMigrationAssembly).GetTypeInfo().Assembly.GetName().Name,
                DatabaseTypes.SqlServer => typeof(SqlServerMigrationAssembly).GetTypeInfo().Assembly.GetName().Name,
                _ => throw new ArgumentOutOfRangeException(nameof(databaseProvider), "不支持的数据库，请检查 appsetting.json")
            };

            return new MigrationAssemblies
            {
                ConfigurationDbMigration = assembly,
                DataProtectionDbMigration = assembly,
                IdentityDbMigration = assembly,
                PersistedGrantDbMigration = assembly
            };
        }
    }
}
