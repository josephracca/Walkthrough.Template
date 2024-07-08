using Application.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string result = JsonConvert.SerializeObject(new ErrorDetail
            {
                ErrorMessage = ex.InnerException != null ?
                    ex.InnerException.Message : ex.Message,
                ErrorType = "Failure"
            });

            statusCode = ex.Data["StatusCode"] != null ? (HttpStatusCode)ex.Data["StatusCode"] : HttpStatusCode.InternalServerError;

            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
