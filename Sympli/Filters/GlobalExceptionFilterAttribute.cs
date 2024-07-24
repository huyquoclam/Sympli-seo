using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Sympli.WebAPI.Filters;


/// <summary>
/// Handle Global exceptions
///  - Adding log
///  - Return Http code based on Application exception
/// </summary>
/// <remarks>
/// Constructor with logger
/// </remarks>
/// <param name="logger"></param>
public sealed class GlobalExceptionFilterAttribute(ILogger<GlobalExceptionFilterAttribute> logger) : ExceptionFilterAttribute
{
    public ILogger<GlobalExceptionFilterAttribute> _logger { get; set; } = logger;

    /// <summary>
    /// Handle global exception
    /// </summary>
    /// <param name="context"></param>
    public override void OnException(ExceptionContext context)
    {
        var errorMessage = context.Exception.InnerException != null ? context.Exception.InnerException.Message : context.Exception.Message;
        switch (context.Exception)
        {
            default:
                context.Result = new JsonResult(context.Exception.ToErrorResponse())
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                break;
        }

        _logger.LogError(context.Exception, errorMessage);
    }
}

public static class ExceptionExtensions
{
    public static ErrorResponse ToErrorResponse(this Exception exception)
    {
        return new ErrorResponse(
            statusCode: (int)HttpStatusCode.InternalServerError,
            message: exception.Message,
            additionalInfo: "",
            errorCode: null);
    }
}