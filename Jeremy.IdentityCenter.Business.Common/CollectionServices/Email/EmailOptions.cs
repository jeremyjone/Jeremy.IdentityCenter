using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace Jeremy.IdentityCenter.Business.Common.CollectionServices.Email
{
    public class EmailOptions
    {
        [EmailAddress]
        public string Address { get; set; }

        public string Host { get; set; } = "";

        public bool EnableSsl { get; set; } = true;

        public bool UseDefaultCredentials { get; set; } = false;

        public NetworkCredential Credentials { get; set; } = new NetworkCredential();

        public int Port { get; set; } = 25; // default port

        public int Timeout { get; set; } = 100000; // 100 second

        public SmtpDeliveryFormat DeliveryFormat { get; set; } = SmtpDeliveryFormat.International;

        public SmtpDeliveryMethod DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;

        public string? PickupDirectoryLocation { get; set; }

        public string? TargetName { get; set; }
    }
}