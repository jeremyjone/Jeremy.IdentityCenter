using IdentityServer4.AspNetIdentity;
using IdentityServer4.Configuration;
using IdentityServer4.Validation;
using Jeremy.IdentityCenter.Business.Providers;
using Jeremy.IdentityCenter.Database.DbContexts;
using Jeremy.Tools.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Jeremy.IdentityCenter.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdcIdentity<TUser, TRole, TKey>(this IServiceCollection services,
            IConfiguration configuration, bool isDevelopment)
            where TUser : IdentityUser<TKey>
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            #region Identity

            services.AddIdentity<TUser, TRole>(options =>
                {
                    // 配置用户条件
                    options.User = new UserOptions
                    {
                        RequireUniqueEmail = true, // 唯一邮箱
                        AllowedUserNameCharacters = null // 允许用户字符
                    };

                    // 配置密码条件
                    options.Password = new PasswordOptions
                    {
                        RequiredLength = 6, // 密码最小长度
                        RequiredUniqueChars = 1, // 出现字母数量
                        RequireDigit = true, // 要求数字
                        RequireLowercase = true, // 要求小写字符
                        RequireUppercase = true, // 要求大写字符
                        RequireNonAlphanumeric = false // 要求特殊字符
                    };

                    // 注册条件
                    options.SignIn = new SignInOptions
                    {
                        RequireConfirmedAccount = configuration["Confirm:ConfirmedAccount"].ToBool(), // 要求确认账户
                        RequireConfirmedEmail = configuration["Confirm:ConfirmedEmail"].ToBool(), // 要求邮箱激活
                        RequireConfirmedPhoneNumber = configuration["Confirm:ConfirmedPhoneNumber"].ToBool() // 要求手机激活
                    };

                    // 锁定账户条件
                    options.Lockout = new LockoutOptions
                    {
                        AllowedForNewUsers = true, // 新账户锁定
                        DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30), // 锁定账户时间
                        MaxFailedAccessAttempts = 3 // 最多尝试次数
                    };

                    // 修改声明名称
                    options.ClaimsIdentity = new ClaimsIdentityOptions
                    {
                        RoleClaimType = "Role",
                        SecurityStampClaimType = "SecurityStamp",
                        UserIdClaimType = "UserId",
                        UserNameClaimType = "UserName",
                        EmailClaimType = "Email"
                    };

                    // 隐私数据保护。启动保护，必须添加下面实现类
                    //options.Stores = new StoreOptions
                    //{
                    //    MaxLengthForKeys = int.MaxValue,
                    //    ProtectPersonalData = true
                    //};

                    // 提供一些 token
                    options.Tokens = new TokenOptions
                    {
                        EmailConfirmationTokenProvider = configuration["Token:ConfirmationEmailToken"],
                        ChangeEmailTokenProvider = configuration["Token:ChangeEmailToken"],
                        ChangePhoneNumberTokenProvider = configuration["Token:ChangePhoneNumberToken"],
                    };
                })
                .AddEntityFrameworkStores<IdcIdentityDbContext>()
                // 启动隐私保护需要添加的实现类，类型需要自行实现。
                // 还需要注意顺序，在 EntityFramework 之后使用。
                //.AddPersonalDataProtection<>() 
                .AddDefaultTokenProviders()
                // 需要对上面提供的 token 进行自定义验证
                .AddTokenProvider<EmailConfirmationTokenProvider<TUser>>(
                    configuration["Token:ConfirmationEmailToken"]);

            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromMinutes(5));

            // 邮箱确认 token 的一些属性
            services.Configure<EmailConfirmationTokenProviderOptions>(options =>
                // 5分钟有效期
                options.TokenLifespan = TimeSpan.FromMinutes(5));

            #endregion



            #region IdentityServer

            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                    options.EmitStaticAudienceClaim = true;

                    if (!string.IsNullOrWhiteSpace(configuration["Url:IssuerUri"]))
                    {
                        options.IssuerUri = configuration["Url:IssuerUri"];
                        options.LowerCaseIssuerUri = true;
                    }

                    // 设置请求地址
                    options.UserInteraction = new UserInteractionOptions
                    {
                        LoginUrl = configuration["Url:Login"],
                        LogoutUrl = configuration["Url:Logout"]
                    };

                    // 扩展发现文档
                    //options.Discovery.CustomEntries.Add("", "");

                    // 指定内部 cookie SameSite 模式
                    //options.Authentication.CookieSameSiteMode = SameSiteMode.Unspecified;
                })
                // 支持密码验证
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator<TUser>>()
                // 配置数据库模式
                .AddAspNetIdentity<TUser>()
                // this adds the config data from DB (clients, resources, CORS)
                //.AddConfigurationStore(options =>
                //{
                //    options.ConfigureDbContext = b =>
                //        b.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                //            sql => sql.MigrationsAssembly(migrationsAssembly));
                //})
                //.AddOperationalStore(options =>
                //{
                //    options.ConfigureDbContext = b =>
                //        b.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                //            sql => sql.MigrationsAssembly(migrationsAssembly));

                //    // this enables automatic token cleanup. this is optional.
                //    options.EnableTokenCleanup = true;
                //});
                .AddConfigurationStore<IdcConfigurationDbContext>()
                .AddOperationalStore<IdcPersistedGrantDbContext>(options => options.EnableTokenCleanup = true);

            // 配置证书
            if (isDevelopment)
            {
                // not recommended for production - you need to store your key material somewhere secure
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                // 生产模式，需要在配置文件中填写证书路径
                var certPath = configuration["Certificates:Path"];
                var certPwd = configuration["Certificates:Password"];
                if (string.IsNullOrWhiteSpace(certPath))
                {
                    throw new Exception("证书配置异常，请检查 appsettings.json。");
                }

                try
                {
                    builder.AddSigningCredential(new X509Certificate2(certPath, certPwd));
                }
                catch (Exception e)
                {
                    throw new Exception("添加证书时发生异常。", e);
                }
            }

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator<TUser>>();

            #endregion


            #region 配置路径

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString(configuration["Url:Login"]);
                options.LogoutPath = new PathString(configuration["Url:Logout"]);
                options.AccessDeniedPath = new PathString("/accessDenied");
            });

            #endregion


            return services;
        }
    }
}
