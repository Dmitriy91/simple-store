using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

#pragma warning disable 1591

namespace Store.Web.Infrastructure.ExceptionHandling
{
    public class ModelStateValidation : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errorMessages = actionContext.ModelState.Values
                .SelectMany(ms => ms.Errors)
                .Select(e => e.ErrorMessage);

                throw new BindingModelValidationException(string.Join(Environment.NewLine, errorMessages));
            }
        }
    }
}

#pragma warning disable 1591
