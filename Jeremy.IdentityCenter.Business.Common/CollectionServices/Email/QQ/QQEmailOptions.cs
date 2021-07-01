using System.Net;

namespace Jeremy.IdentityCenter.Business.Common.CollectionServices.Email.QQ
{
    public class QQEmailOptions
    {
        public string Address { get; set; }

        public NetworkCredential Credentials { get; set; }

        public bool EnableSsl { get; set; } = true;

        public bool UseDefaultCredentials { get; set; } = false;
    }
}