using System;

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
