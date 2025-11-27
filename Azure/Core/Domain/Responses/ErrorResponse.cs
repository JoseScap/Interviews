using System.Net;

namespace Core.Domain.Responses;

public class ErrorResponse
{
    public string ErrorCode { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public HttpStatusCode StatusCode { get; set; }
}

