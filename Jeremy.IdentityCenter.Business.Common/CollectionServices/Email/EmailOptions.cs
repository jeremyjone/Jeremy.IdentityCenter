using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace Jeremy.IdentityCenter.Business.Common.CollectionServices.Email
{
    public class EmailOptions
    {
        private int _port;

        [EmailAddress]
        public string Address { get; set; }

        public string Host { get; set; } = "";

        public bool EnableSsl { get; set; } = true;

        public bool UseDefaultCredentials { get; set; } = false;

        public NetworkCredential Credentials { get; set; } = new NetworkCredential();

        public int Port
        {
            get => _port == default ? EnableSsl ? 465 : 25 : _port;
            set => _port = value;
        }

        public int Timeout { get; set; } = 100000; // 100 second

        public SmtpDeliveryFormat DeliveryFormat { get; set; } = SmtpDeliveryFormat.SevenBit;

        public SmtpDeliveryMethod DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;

        public string? PickupDirectoryLocation { get; set; }

        public string? TargetName { get; set; } = "SMTPSVC/";
    }
}