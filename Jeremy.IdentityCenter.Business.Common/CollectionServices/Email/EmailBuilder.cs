using Microsoft.Extensions.DependencyInjection;

namespace Jeremy.IdentityCenter.Business.Common.CollectionServices.Email
{
    public class EmailBuilder<TEmailSender> where TEmailSender : class
    {
        public EmailBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public IServiceCollection Build()
        {
            return Services.AddSingleton<TEmailSender>();
        }
    }
}