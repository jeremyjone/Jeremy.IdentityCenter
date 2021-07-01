using Jeremy.IdentityCenter.Authorization;
using Jeremy.IdentityCenter.Business.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Jeremy.IdentityCenter.Extensions
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection AddIdcAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyConstants.Super,
                    policy =>
                    {
                        policy.Requirements.Add(new ClaimRequirement("role", "super"));
                        policy.RequireRole("super");
                    });

                // 可以通过 Builder 的方式添加策略
                //options.DefaultPolicy = new AuthorizationPolicyBuilder()
                //    .AddRequirements(new ClaimRequirement("role", "super")).Build();
            });

            services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();

            return services;
        }
    }
}
