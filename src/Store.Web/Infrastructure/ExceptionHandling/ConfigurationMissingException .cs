using System;
using System.Configuration;

#pragma warning disable 1591

namespace Store.Web.Infrastructure.ExceptionHandling
{
    [Serializable]
    public class ConfigurationMissingException : ConfigurationErrorsException
    {
        public ConfigurationMissingException(string message)
            : base(message)
        {
        }

        public ConfigurationMissingException(string message, string filename, int line)
            : base(message, filename, line)
        {
        }
    }
}

#pragma warning restore 1591
