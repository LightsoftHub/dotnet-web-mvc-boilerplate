using CleanArch.eCode.WebApp.Core.ViewModels;
using Light.Exceptions;
using System.Net;
using System.Text.Json;

namespace CleanArch.eCode.WebApp.Infrastructure.Middlewares;

public class ErrorHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            bool isAjaxCall = context.Request.Headers.XRequestedWith == "XMLHttpRequest";
            if (isAjaxCall)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var messages = ex.Message;

                switch (ex)
                {
                    /*
                    case ValidationException e:
                        response.StatusCode = (int)e.StatusCode;
                        responseModel = e.ErrorMessages?.JoinToString("<br>");
                        break;
                    */
                    case ExceptionBase e:
                        // custom application error
                        response.StatusCode = (int)e.StatusCode;
                        messages += "<br>" + e.Message;
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var responseModel = WebResult.Error(messages);
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
            else
            {
                /*
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

                if (ex is NotFoundException || ex is KeyNotFoundException)
                {
                    statusCode = HttpStatusCode.NotFound;
                }
                else if (ex is UnauthorizedException || ex is UnauthorizedAccessException)
                {
                    if (ex.Message == "Method reserved for in-scope initialization")
                        statusCode = HttpStatusCode.NotFound;
                    else
                        statusCode = HttpStatusCode.Unauthorized;
                }

                switch (statusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        context.Response.Redirect("/Account/AccessDenied");
                        break;
                    case HttpStatusCode.NotFound:
                        context.Response.Redirect("/404");
                        break;
                    default:
                        context.Response.Redirect("/Home/Error/{0}");
                        break;
                }
                */

                await _next(context);
            }
        }
    }
}