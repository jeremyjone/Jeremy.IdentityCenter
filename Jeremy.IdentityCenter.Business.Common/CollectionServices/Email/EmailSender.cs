using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Business.Common.CollectionServices.Email
{
    public class EmailSender : IEmailSender
    {
        protected readonly IConfiguration Configuration;
        private SmtpClient Client { get; }
        private string Address { get; }

        public EmailSender(IConfiguration configuration, EmailOptions options)
        {
            Configuration = configuration;
            Address = options.Address;
            Client = new SmtpClient
            {
                Host = options.Host,
                EnableSsl = options.EnableSsl,
                UseDefaultCredentials = options.UseDefaultCredentials,
                Credentials = options.Credentials,
                Port = options.Port,
                Timeout = options.Timeout,
                DeliveryFormat = options.DeliveryFormat,
                DeliveryMethod = options.DeliveryMethod,
                PickupDirectoryLocation = options?.PickupDirectoryLocation,
                TargetName = options?.TargetName,
            };
        }

        public virtual async Task SendEmailAsync(string address, string displayName, string subject, string body,
            bool isBodyHtml = false)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(Address, displayName, Encoding.UTF8),
            };

            mail.To.Add(new MailAddress(address));
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = isBodyHtml;

            await Client.SendMailAsync(mail);
        }
    }
}
