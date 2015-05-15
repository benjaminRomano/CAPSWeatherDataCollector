using System.Net;
using System.Web;
using System.Web.Http.ExceptionHandling;
using CAPSWeatherAPI.Utilities;

namespace CAPSWeatherAPI.Handlers
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var exception = context.Exception;

            var httpException = exception as HttpException;
            if (httpException != null)
            {
                context.Result = new CustomErrorResult(context.Request,
                    (HttpStatusCode)httpException.GetHttpCode(),
                    httpException.Message);
                return;
            }

            // Return HttpStatusCode for other types of exception.

            context.Result = new CustomErrorResult(context.Request,
                HttpStatusCode.InternalServerError,
                exception.Message);
        }
    }
}