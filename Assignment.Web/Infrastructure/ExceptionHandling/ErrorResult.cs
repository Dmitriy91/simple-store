using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Assignment.Web.Infrastructure.ExceptionHandling
{
    public class ErrorResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _requestMessage;

        public ErrorResult(HttpRequestMessage requestMessage, HttpStatusCode statusCode, string errorMessage)
        {
            _requestMessage = requestMessage;
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_requestMessage.CreateErrorResponse(StatusCode, ErrorMessage));
        }

        public string ErrorMessage
        {
            get;
            private set;
        }

        public HttpStatusCode StatusCode
        {
            get;
            private set;
        }
    }
}
