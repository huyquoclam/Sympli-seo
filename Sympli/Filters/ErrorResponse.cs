namespace Sympli.WebAPI.Filters;

public class ErrorResponse(
    int statusCode,
    string message,
    string additionalInfo,
    string? errorCode = null)
{
    public int StatusCode { get; set; } = statusCode;
    public DateTimeOffset Datetime { get; set; } = DateTimeOffset.UtcNow;
    public string AdditionalInfo { get; set; } = additionalInfo;
    public string Message { get; set; } = message;
    public string? ErrorCode { get; set; } = errorCode;
}
