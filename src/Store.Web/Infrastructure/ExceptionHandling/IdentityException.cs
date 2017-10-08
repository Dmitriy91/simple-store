using System;

#pragma warning disable 1591

namespace Store.Web.Infrastructure.ExceptionHandling
{
    [Serializable]
    public class IdentityException : Exception
    {
        public IdentityException(string message)
            : base(message)
        {
        }
    }
}

#pragma warning restore 1591
