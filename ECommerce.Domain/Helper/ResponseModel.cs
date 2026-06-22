
using System.Net;

namespace ECommerce.Domain.Helper;

public class ResponseModel<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Result { get; set; }

    /* ---------- SUCCESS ---------- */
    public static ResponseModel<T> Success(
        T result,
        string message = "Success",
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new ResponseModel<T>
        {
            StatusCode = statusCode,
            Message = message,
            Result = result,
        };
    }

    /* ---------- FAIL ---------- */
    public static ResponseModel<T> Fail(
        string message = "Fail",
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ResponseModel<T>
        {
            StatusCode = statusCode,
            Message = message,
            Result = default,
        };
    }
}
