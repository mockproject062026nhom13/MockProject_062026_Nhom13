using FluentValidation;
using NursingHome.Application.Common;

namespace NursingHome.Api.Middleware;

/// <summary>
/// Terminal error boundary for the request pipeline. Catches every exception that
/// bubbles up from downstream middleware/handlers and maps it — via a single switch
/// expression — to a consistent <see cref="ApiResponse{T}"/> payload. Known exception
/// types get a meaningful status code; anything unrecognised falls through to a generic
/// 500 so no internal detail leaks to the client. Register this first so it wraps everything.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var response = MapToResponse(exception);

            // 5xx means an unexpected server-side fault worth an error log; everything
            // else is an expected, client-driven outcome and only warrants a warning.
            if (response.StatusCode >= StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            }
            else
            {
                _logger.LogWarning(exception, "Request failed ({StatusCode}) for {Method} {Path}", response.StatusCode, context.Request.Method, context.Request.Path);
            }

            await WriteResponseAsync(context, response);
        }
    }

    /// <summary>
    /// Maps a caught exception to the error response returned to the client. Add new
    /// arms here as domain-specific exceptions are introduced. Messages are deliberately
    /// generic (never <c>exception.Message</c>) so internal details never reach the client
    /// — the exception itself is logged separately for diagnostics.
    /// </summary>
    private static ApiResponse<object> MapToResponse(Exception exception) => exception switch
    {
        ValidationException validationException => ApiResponse<object>.CreateError(
            StatusCodes.Status400BadRequest,
            "One or more validation errors occurred.",
            validationException.Errors
                .Select(failure => new ApiError(failure.PropertyName, failure.ErrorMessage))
                .ToList()),

        ArgumentException => ApiResponse<object>.CreateError(
            StatusCodes.Status400BadRequest,
            "The request contains one or more invalid arguments."),

        UnauthorizedAccessException => ApiResponse<object>.CreateError(
            StatusCodes.Status401Unauthorized,
            "You are not authorized to perform this action."),

        KeyNotFoundException => ApiResponse<object>.CreateError(
            StatusCodes.Status404NotFound,
            "The requested resource was not found."),

        NotImplementedException => ApiResponse<object>.CreateError(
            StatusCodes.Status501NotImplemented,
            "This functionality is not implemented."),

        // Client aborted the request (e.g. navigated away). 499 is the conventional
        // "client closed request" code; there is usually no live connection to write to.
        OperationCanceledException => ApiResponse<object>.CreateError(
            StatusCodes.Status499ClientClosedRequest,
            "The request was canceled."),

        _ => ApiResponse<object>.CreateError(
            StatusCodes.Status500InternalServerError,
            "An unexpected error occurred while processing your request."),
    };

    private static async Task WriteResponseAsync(HttpContext context, ApiResponse<object> response)
    {
        // If the response has already started, headers are locked and we can no longer
        // shape the error payload — let it surface rather than corrupting the stream.
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.StatusCode = response.StatusCode;
        // WriteAsJsonAsync uses the app's configured JSON options (camelCase web defaults),
        // matching what the controllers produce, and sets the content type.
        await context.Response.WriteAsJsonAsync(response);
    }
}
