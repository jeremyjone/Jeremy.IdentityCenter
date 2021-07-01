using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jeremy.IdentityCenter.Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddIdcSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            // 添加文档
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"{configuration["Api:ApiName"]} Document",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "jeremyjone@qq.com",
                        Name = "Jeremy Jone",
                        Url = new Uri("https://www.jeremyjone.com")
                    }
                });

                // 启动swagger验证
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{configuration["Api:IdsHost"]}/connect/authorize"),
                            TokenUrl = new Uri($"{configuration["Api:IdsHost"]}/connect/token"),
                            Scopes = new Dictionary<string, string> {
                                { configuration["Api:OidcApiName"], configuration["Api:ApiName"] }
                            }
                        }
                    }
                });
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }

        public static IApplicationBuilder UseIdcSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", configuration["Api:ApiName"]);

                c.OAuthClientId(configuration["Api:OidcSwaggerUIClientId"]);
                c.OAuthAppName(configuration["Api:ApiName"]);
                c.OAuthUsePkce();
            });

            return app;
        }
    }

    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public IConfiguration Configuration { get; }

        public AuthorizeCheckOperationFilter(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize = context.MethodInfo.DeclaringType != null && (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                                                                            || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any());

            if (hasAuthorize)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"
                                }
                            }
                        ] = new[] {Configuration["Api:OidcApiName"] }
                    }
                };

            }
        }
    }
}
