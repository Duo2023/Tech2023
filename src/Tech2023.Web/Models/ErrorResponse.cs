using System.Net;

namespace Tech2023.Web.Models;

public class ErrorResponse
{
    public HttpStatusCode StatusCode { get; protected init; }

    public string? Message { get; protected init; }

    public static ErrorResponse Create(HttpStatusCode code, string? message = null)
    {
        return new ErrorResponse()
        {
            StatusCode = code,
            Message = message,
        };
    }
}
