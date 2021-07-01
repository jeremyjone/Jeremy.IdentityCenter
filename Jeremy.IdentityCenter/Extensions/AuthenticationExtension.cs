using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Jeremy.IdentityCenter.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddIdcAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authentication = services.AddAuthentication();

            // 添加 google 认证
            if (configuration["Authentication:Google:ClientId"] != null)
            {
                authentication.AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                });
            }

            // 添加 Microsoft 认证
            if (configuration["Authentication:Microsoft:ClientId"] != null)
            {
                authentication.AddMicrosoftAccount("Microsoft", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = configuration["Authentication:Microsoft:ClientId"];
                    options.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
                });
            }

            // 自定义外部认证
            authentication.AddOpenIdConnect("oidc", "OpenID Connect", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.SignOutScheme = IdentityServerConstants.SignoutScheme;

                options.Authority = "https://demo.identityserver.io/";
                options.ClientId = "implicit";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });


            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            return services;
        }
    }
}
