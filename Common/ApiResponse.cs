using System.Collections.Generic;

namespace Models.Common;

public class ApiResponse<T>
{
    public bool Success { get; init; }

    public int StatusCode { get; init; }

    public string Message { get; init; } = string.Empty;

    public T? Data { get; init; }

    public IReadOnlyList<ApiError>? Errors { get; init; }

    public PaginationMetadata? Pagination { get; init; }

    public static ApiResponse<T> CreateSuccess(T? data, int statusCode = 200, string message = "Operation completed successfully.", PaginationMetadata? pagination = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            StatusCode = statusCode,
            Message = message,
            Data = data,
            Errors = null,
            Pagination = pagination
        };
    }

    public static ApiResponse<T> CreateError(int statusCode, string message, IReadOnlyList<ApiError>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Data = default,
            Errors = errors,
            Pagination = null
        };
    }
}
