using Microsoft.Extensions.DependencyInjection;
using System;

namespace Jeremy.IdentityCenter.Business.Common.CollectionServices.Email.QQ
{
    public static class QQEmailExtension
    {
        public static IServiceCollection AddQQEmail<TEmailSender>(this EmailBuilder<TEmailSender> builder,
            Action<QQEmailOptions> optionsAction) where TEmailSender : class
        {
            var qqOptions = new QQEmailOptions();
            optionsAction(qqOptions);

            var options = new EmailOptions
            {
                Address = qqOptions.Address,
                Host = "smtp.qq.com",
                Credentials = qqOptions.Credentials,
                EnableSsl = qqOptions.EnableSsl,
                UseDefaultCredentials = qqOptions.UseDefaultCredentials
            };

            builder.Services.AddSingleton(options);
            return builder.Build();
        }
    }
}
