using MediatR;
using ModelFilter.Domain.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;

namespace ModelFilter.Api.Configuration
{
    public class ErrorHandleMiddlewares
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly INotification _notification;

        public ErrorHandleMiddlewares(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }
        public async Task Invoke(HttpContext httpContext, IMediator mediator)
        {
            try
            {
                await _requestDelegate(httpContext);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(httpContext, ex, mediator);
            }
        }

        private static Task HandleExceptionAsync(HttpContext httpContext, Exception ex, IMediator mediator)
        {
            var response = httpContext.Response;
            response.ContentType = "application/json";

            var statusCode = ex is ValidationException ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError;

            var trace = new StackTrace(ex, true);

            var separetString = ex.Message.Split('\n').Where(x => x != "").ToList();

            var result = new ReturnDefault<object>()
            {
                Errors = separetString,
                Sucess = false
            };

            var jsonResult = JsonConvert.SerializeObject(result) ?? "";
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)statusCode;

            return httpContext.Response.WriteAsync(jsonResult);
        }
    }
}
