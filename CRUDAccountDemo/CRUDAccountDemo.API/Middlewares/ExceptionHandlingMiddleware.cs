namespace CRUDAccountDemo.API.Middlewares;

using CRUDAccountDemo.API.Common;
using BusinessException = CRUDAccountDemo.Business.Exceptions.BusinessException;
using NotFoundException = CRUDAccountDemo.Business.Exceptions.NotFoundException;
using ValidationException = CRUDAccountDemo.Business.Exceptions.ValidationException;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteErrorResponseAsync(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (ValidationException ex)
        {
            var errors = new[] { new ErrorDetail(ex.Field, ex.Message) };
            await WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, "Validation failed.", errors);
        }
        catch (BusinessException ex)
        {
            var errors = new[] { new ErrorDetail("business", ex.Message) };
            await WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, ex.Message, errors);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");
            await WriteErrorResponseAsync(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    private static async Task WriteErrorResponseAsync(
        HttpContext context,
        int statusCode,
        string message,
        IEnumerable<ErrorDetail>? errors = null)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new ApiResponse<object?>(false, statusCode, message, null, errors, null);
        await context.Response.WriteAsJsonAsync(response);
    }
}
