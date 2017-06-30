using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

#pragma warning disable 1591

namespace Assignment.Web.Infrastructure.Swagger
{
    public class ShortenNameOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var spliter = new[] { '.' };

            foreach (var oper in operation.parameters)
                oper.name = oper.name.Split(spliter).Last();
        }
    }
}

#pragma warning restore 1591
