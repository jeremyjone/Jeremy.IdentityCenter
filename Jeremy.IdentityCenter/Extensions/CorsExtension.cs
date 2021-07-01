using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Jeremy.IdentityCenter.Extensions
{
    public static class CorsExtension
    {
        public static IServiceCollection AddIdcCors(this IServiceCollection services, IdcCorsConfiguration configuration = null)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        if (configuration?.CorsAllowOrigins.Any() == true)
                        {
                            builder.WithOrigins(configuration.CorsAllowOrigins);
                        }
                        else
                        {
                            builder.AllowAnyOrigin();
                        }

                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });

            return services;
        }
    }

    public class IdcCorsConfiguration
    {
        public string[] CorsAllowOrigins { get; set; }
    }
}
