using System;

#pragma warning disable 1591

namespace Assignment.Web.Infrastructure.ExceptionHandling
{
    public class BindingModelValidationException : Exception
    {
        public BindingModelValidationException(string message)
            : base(message)
        {
        }
    }
}

#pragma warning restore 1591
