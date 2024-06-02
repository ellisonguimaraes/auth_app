using AuthApp.Domain.Utils;

namespace AuthApp.Application.Models;

public static class Responses
{
    public static GenericHttpResponse<T> WithError<T>(int statusCode, IEnumerable<string> errors) where T : class
        => new GenericHttpResponse<T>
        {
            StatusCode = statusCode,
            Errors = errors,
        };

    public static GenericHttpResponse<T> WithError<T>(int statusCode, string error) where T : class
        => new GenericHttpResponse<T>
        {
            StatusCode = statusCode,
            Errors = new List<string> { error }
        };

    public static GenericHttpResponse<object> WithError(int statusCode, IEnumerable<string> errors)
        => new GenericHttpResponse<object>
        {
            StatusCode = statusCode,
            Errors = errors,
        };

    public static GenericHttpResponse<object> WithError(int statusCode, string error)
        => new GenericHttpResponse<object>
        {
            StatusCode = statusCode,
            Errors = new List<string> { error }
        };

    public static GenericHttpResponse<T> WithSuccess<T>(int statusCode, T data) where T : class
        => new GenericHttpResponse<T>
        {
            StatusCode = statusCode,
            Data = data,
        };

    public static GenericHttpResponse<object> WithSuccess(int statusCode)
        => new GenericHttpResponse<object>
        {
            StatusCode = statusCode,
            Data = null,
        };
}
