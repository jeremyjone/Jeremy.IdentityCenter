using System;

namespace Jeremy.IdentityCenter.Business.Common.Models
{
    public class NullResultException : Exception
    {
        public string ErrorKey { get; set; }

        public NullResultException(string message) : base(message)
        {

        }

        public NullResultException(string message, string errorKey) : base(message)
        {
            ErrorKey = errorKey;
        }

        public NullResultException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
