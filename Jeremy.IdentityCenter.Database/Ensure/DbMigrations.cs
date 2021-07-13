using IdentityModel;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using Jeremy.IdentityCenter.Database.Constants;
using Jeremy.IdentityCenter.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Ensure
{
    public static class DbMigrations
    {
        public static async Task ApplyMigrationsAsync<TIdentityDbContext, TConfigurationDbContext,
            TPersistedGrantDbContext, TDataProtectionDbContext, TUser, TRole, TKey>(
            IServiceProvider serviceProvider)
            where TIdentityDbContext : DbContext
            where TConfigurationDbContext : DbContext, IConfigurationDbContext
            where TPersistedGrantDbContext : DbContext, IPersistedGrantDbContext
            where TDataProtectionDbContext : DbContext
            where TUser : IdentityUser<TKey>, new()
            where TRole : IdentityRole<TKey>, new()
            where TKey : IEquatable<TKey>
        {
            var scope = serviceProvider.CreateScope();
            await EnsureDatabasesAsync<TIdentityDbContext, TConfigurationDbContext,
                TPersistedGrantDbContext, TDataProtectionDbContext>(scope.ServiceProvider);
            await EnsureSeedDataAsync<TConfigurationDbContext, TUser, TRole, TKey>(scope.ServiceProvider);
        }

        private static async Task EnsureDatabasesAsync<TIdentityDbContext, TConfigurationDbContext,
            TPersistedGrantDbContext, TDataProtectionDbContext>(
            IServiceProvider serviceProvider)
            where TIdentityDbContext : DbContext
            where TConfigurationDbContext : DbContext
            where TPersistedGrantDbContext : DbContext
            where TDataProtectionDbContext : DbContext
        {

            #region 运行前须知

            /*
             * 在整体运行之前，确保存在对应数据库类库中 /Migrations 的内容。如果没有，需要先通过命令创建，否则以下代码不会创建数据表。
             * 通过包管理器命令行创建即可，注意默认项目选择对应的数据库类库
             *
             * 【数据库的选择】运行下面代码之前，确保数据库类库与 appsettings.json 中的 DatabaseProvider.Type 值对应，否则可能会有异常。
             *
             * PM> add-migration InitDb -c IdcPersistedGrantDbContext -o Migrations/IdcPersistedGrantDb
             * Build started...
             * Build succeeded.
             * To undo this action, use Remove-Migration.
             *
             * PM> add-migration InitDb -c IdcConfigurationDbContext -o Migrations/IdcConfigurationDb
             * Build started...
             * Build succeeded.
             * To undo this action, use Remove-Migration.
             *
             * PM> add-migration InitDb -c IdcIdentityDbContext -o Migrations/IdcIdentityDb
             * Build started...
             * Build succeeded.
             * To undo this action, use Remove-Migration.
             *
             * PM> add-migration InitDb -c IdcDataProtectionDbContext -o Migrations/IdcDataProtectionDb
             * Build started...
             * Build succeeded.
             * To undo this action, use Remove-Migration.
             *
             *
             * 如需手动更新数据库，使用 `update-database -context <DbContext>` 命令。
             */

            #endregion


            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            {
                Console.WriteLine("Creating IdentityDbContext...");
                await using var context = scope.ServiceProvider.GetRequiredService<TIdentityDbContext>();
                await context.Database.MigrateAsync();
                Console.WriteLine("IdentityDbContext was created!");
            }

            {
                Console.WriteLine("Creating ConfigurationDbContext...");
                await using var context = scope.ServiceProvider.GetRequiredService<TConfigurationDbContext>();
                await context.Database.MigrateAsync();
                Console.WriteLine("ConfigurationDbContext was created!");
            }

            {
                Console.WriteLine("Creating PersistedGrantDbContext...");
                await using var context = scope.ServiceProvider.GetRequiredService<TPersistedGrantDbContext>();
                await context.Database.MigrateAsync();
                Console.WriteLine("PersistedGrantDbContext was created!");
            }

            {
                Console.WriteLine("Creating IdcDataProtectionDbContext...");
                await using var context = scope.ServiceProvider.GetRequiredService<TDataProtectionDbContext>();
                await context.Database.MigrateAsync();
                Console.WriteLine("IdcDataProtectionDbContext was created!");
            }

            //using (var context = scope.ServiceProvider.GetRequiredService<TLogDbContext>())
            //{
            //    await context.Database.MigrateAsync();
            //}

            //using (var context = scope.ServiceProvider.GetRequiredService<TAuditLogDbContext>())
            //{
            //    await context.Database.MigrateAsync();
            //}
        }

        private static async Task
            EnsureSeedDataAsync<TConfigurationDbContext, TUser, TRole, TKey>(IServiceProvider serviceProvider)
            where TConfigurationDbContext : DbContext, IConfigurationDbContext
            where TUser : IdentityUser<TKey>, new()
            where TRole : IdentityRole<TKey>, new()
            where TKey : IEquatable<TKey>
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TConfigurationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<TRole>>();

            // add Clients
            Console.WriteLine("Creating Clients...");
            foreach (var client in TestData.Clients)
            {
                if (await context.Clients.AnyAsync(c => c.ClientId == client.ClientId)) continue;

                await context.Clients.AddAsync(client.ToEntity());
            }
            Console.WriteLine("Clients was created!");


            // add IdentityResources
            Console.WriteLine("Creating IdentityResources...");
            foreach (var identityResource in TestData.IdentityResources)
            {
                if (await context.IdentityResources.AnyAsync(ir => ir.Name == identityResource.Name)) continue;

                await context.IdentityResources.AddAsync(identityResource.ToEntity());
            }
            Console.WriteLine("IdentityResources was created!");


            // add ApiResources
            Console.WriteLine("Creating ApiResources...");
            foreach (var resource in TestData.ApiResources)
            {
                if (await context.ApiResources.AnyAsync(r => r.Name == resource.Name)) continue;

                await context.ApiResources.AddAsync(resource.ToEntity());
            }
            Console.WriteLine("ApiResources was created!");


            // add ApiScopes
            Console.WriteLine("Creating ApiScopes...");
            foreach (var apiScope in TestData.ApiScopes)
            {
                if (await context.ApiScopes.AnyAsync(s => s.Name == apiScope.Name)) continue;

                await context.ApiScopes.AddAsync(apiScope.ToEntity());
            }
            Console.WriteLine("ApiScopes was created!");

            await context.SaveChangesAsync();




            // add Roles
            Console.WriteLine("Creating Roles...");
            foreach (var role in TestData.Roles)
            {
                if (await roleManager.RoleExistsAsync(role.Name)) continue;

                Console.WriteLine($"Adding {role.Name} role...");
                var res = await roleManager.CreateAsync(role as TRole);
                if (!res.Succeeded)
                {
                    throw new Exception(res.Errors.First().Description);
                }
            }
            Console.WriteLine("Roles was created!");


            // add Users
            Console.WriteLine("Creating Users...");
            foreach (var u in TestData.Users)
            {
                if (await userManager.FindByNameAsync(u.UserName) != default) continue;

                var user = new IdcIdentityUser
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    EmailConfirmed = true,
                    BirthDate = u.BirthDate,
                    Sex = u.Sex,
                    NickName = u.NickName
                };
                Console.WriteLine($"Adding {user.UserName} user...");
                var res = await userManager.CreateAsync(user as TUser, Configuration.DefaultPassword);
                if (!res.Succeeded)
                {
                    throw new Exception(res.Errors.First().Description);
                }

                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Name, user.NormalizedUserName),
                    new Claim(JwtClaimTypes.Email, user.Email)
                };
                claims.AddRange(u.UserRoles.Select(ur => new Claim(JwtClaimTypes.Role, ur.Role.Name)));

                Console.WriteLine("Adding claims to user...");
                res = await userManager.AddClaimsAsync(user as TUser, claims);
                if (!res.Succeeded)
                {
                    throw new Exception(res.Errors.First().Description);
                }

                var roles = u.UserRoles.Select(x => x.Role.Name).ToList();
                Console.WriteLine("Adding roles to user...");
                res = await userManager.AddToRolesAsync(user as TUser, roles);
                if (!res.Succeeded)
                {
                    throw new Exception(res.Errors.First().Description);
                }
            }
            Console.WriteLine("Users was created...");
        }
    }
}
