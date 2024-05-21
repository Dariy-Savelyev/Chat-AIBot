using ChatBot.CrossCutting.Exceptions;
using ChatBot.CrossCutting.Models;
using System.Text.Json;

namespace ChatBot.Web.Middlewares;

internal sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger = context.RequestServices.GetService<ILogger<ExceptionHandlingMiddleware>>()!;
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            ArgumentValidationException => StatusCodes.Status422UnprocessableEntity,
            ConflictException => StatusCodes.Status409Conflict,
            NotFoundException => StatusCodes.Status404NotFound,
            GoneException => StatusCodes.Status410Gone,
            ForbiddenException => StatusCodes.Status403Forbidden,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            FailedDependencyException => StatusCodes.Status424FailedDependency,
            _ => StatusCodes.Status500InternalServerError
        };

    private static IReadOnlyCollection<ResponseError> GetErrors(Exception exception)
    {
        IReadOnlyCollection<ResponseError> errors;
        if (exception is BaseException argumentValidationException)
        {
            errors = argumentValidationException.Errors;
        }
        else
        {
            errors = [new([exception.ToString()])];
        }

        return errors;
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogCritical(exception, null);
        }
        else
        {
            _logger.LogDebug(exception, null);
        }

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(GetErrors(exception)));
    }
}