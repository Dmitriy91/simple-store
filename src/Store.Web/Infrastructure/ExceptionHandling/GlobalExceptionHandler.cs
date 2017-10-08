using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http.ExceptionHandling;
using log4net;

#pragma warning disable 1591

namespace Store.Web.Infrastructure.ExceptionHandling
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        private static readonly ILog _logger;

        private static string FormatFailedRequestInfo(HttpRequestMessage request, string httpStatusCode, string message)
        {
            return string.Format("Api request failed {0} \r\n http status code: {1}, message: {2}",
                request.RequestUri, httpStatusCode, message);
        }

        static GlobalExceptionHandler()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }

        public override void Handle(ExceptionHandlerContext context)
        {
            HttpException httpException = context.Exception as HttpException;

            if (httpException != null)
            {
                context.Result = new ErrorResult(context.Request,
                    (HttpStatusCode)httpException.GetHttpCode(), httpException.Message);
            }
            else if (context.Exception is BindingModelValidationException)
            {
                context.Result = new ErrorResult(context.Request, HttpStatusCode.BadRequest, context.Exception.Message);
            }
            else
            {
                var argumentExceptionTypes = new Type[]
                {
                    typeof(ArgumentException),
                    typeof(ArgumentNullException),
                    typeof(ArgumentOutOfRangeException)
                };

                if (argumentExceptionTypes.Any(exceptionType => context.Exception.GetType() == exceptionType))
                    context.Result = new ErrorResult(context.Request, HttpStatusCode.BadRequest, context.Exception.Message);
                else
                    context.Result = new ErrorResult(context.Request, HttpStatusCode.InternalServerError, context.Exception.Message);
            }

            _logger.Error(FormatFailedRequestInfo(context.Request, ((ErrorResult)context.Result).StatusCode.ToString(),
                ((ErrorResult)context.Result).ErrorMessage), context.Exception);
        }
    }
}

#pragma warning restore 1591
