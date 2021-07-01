using Microsoft.Extensions.DependencyInjection;
using System;

namespace Jeremy.IdentityCenter.Business.Common.CollectionServices.Email
{
    public static class EmailExtension
    {
        public static IServiceCollection AddEmailSender<TEmailSender>(this IServiceCollection services,
            Action<EmailOptions> optionsAction) where TEmailSender : class
        {
            var options = new EmailOptions();
            optionsAction(options);

            services.AddSingleton(options);
            return new EmailBuilder<TEmailSender>(services).Build();
        }

        public static EmailBuilder<TEmailSender> AddEmailSender<TEmailSender>(this IServiceCollection services)
            where TEmailSender : class
        {
            return new EmailBuilder<TEmailSender>(services);
        }
    }
}
