using System;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace Assignment.Web.Infrastructure
{
    public static class ApiControllerHelpers
    {
        public static string GetModelStateErrorMessage(this ApiController controller)
        {
            var errorMessages = controller.ModelState.Values
                .SelectMany(ms => ms.Errors)
                .Select(e => e.ErrorMessage);

            return string.Join(Environment.NewLine, errorMessages);
        }
    }
}
