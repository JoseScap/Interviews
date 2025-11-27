using System.Net;

namespace Core.Domain.Exceptions;

public record ExceptionDefinition(string ErrorCode, HttpStatusCode StatusCode);

public static class ExceptionDefinitions
{
    public static readonly ExceptionDefinition BAD_REQUEST = new("BAD_REQUEST", HttpStatusCode.BadRequest);
    public static readonly ExceptionDefinition UNAUTHORIZED = new("UNAUTHORIZED", HttpStatusCode.Unauthorized);
    public static readonly ExceptionDefinition FORBIDDEN = new("FORBIDDEN", HttpStatusCode.Forbidden);
    public static readonly ExceptionDefinition NOT_FOUND = new("NOT_FOUND", HttpStatusCode.NotFound);
    public static readonly ExceptionDefinition INTERNAL_SERVER_ERROR = new("INTERNAL_SERVER_ERROR", HttpStatusCode.InternalServerError);
    public static readonly ExceptionDefinition NOT_IMPLEMENTED = new("NOT_IMPLEMENTED", HttpStatusCode.NotImplemented);
}

public class ApiException : System.Exception
{
    public string ErrorCode { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public Dictionary<string, string> Details { get; set; }

    public ApiException(string message, Dictionary<string, string> details, string errorCode, HttpStatusCode statusCode) : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
        Details = details;
    }

    public static ApiException BadRequest(string message, Dictionary<string, string>? details = null)
    {
        return new ApiException(
            message, details ?? new Dictionary<string, string>(),
            ExceptionDefinitions.BAD_REQUEST.ErrorCode,
            ExceptionDefinitions.BAD_REQUEST.StatusCode);
    }

    public static ApiException Unauthorized(string message, Dictionary<string, string>? details = null)
    {
        return new ApiException(
            message, details ?? new Dictionary<string, string>(),
            ExceptionDefinitions.UNAUTHORIZED.ErrorCode,
            ExceptionDefinitions.UNAUTHORIZED.StatusCode);
    }

    public static ApiException Forbidden(string message, Dictionary<string, string>? details = null)
    {
        return new ApiException(
            message, details ?? new Dictionary<string, string>(),
            ExceptionDefinitions.FORBIDDEN.ErrorCode,
            ExceptionDefinitions.FORBIDDEN.StatusCode);
    }

    public static ApiException NotFound(string message, Dictionary<string, string>? details = null)
    {
        return new ApiException(
            message, details ?? new Dictionary<string, string>(),
            ExceptionDefinitions.NOT_FOUND.ErrorCode,
            ExceptionDefinitions.NOT_FOUND.StatusCode);
    }

    public static ApiException InternalServerError(string message, Dictionary<string, string>? details = null)
    {
        return new ApiException(
            message, details ?? new Dictionary<string, string>(),
            ExceptionDefinitions.INTERNAL_SERVER_ERROR.ErrorCode,
            ExceptionDefinitions.INTERNAL_SERVER_ERROR.StatusCode);
    }

    public static ApiException NotImplemented(string message, Dictionary<string, string>? details = null)
    {
        return new ApiException(
            message, details ?? new Dictionary<string, string>(),
            ExceptionDefinitions.NOT_IMPLEMENTED.ErrorCode,
            ExceptionDefinitions.NOT_IMPLEMENTED.StatusCode);
    }
}