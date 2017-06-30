using Swashbuckle.Swagger;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

#pragma warning disable 1591

namespace Assignment.Web.Infrastructure.Swagger
{
    public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var authorizeAttrs = 
                apiDescription.GetControllerAndActionAttributes<AuthorizeAttribute>();
            var allowAnonymousAttrs =
                apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>();

            if (authorizeAttrs.Any() && !allowAnonymousAttrs.Any())
            {
                operation.parameters.Add(new Parameter
                {
                    name = "Authorization",
                    @in = "header",
                    description = "Authorization header",
                    required = true,
                    type = "string",
                    @default = "Bearer " 
                });
            }
        }
    }
}

#pragma warning restore 1591
