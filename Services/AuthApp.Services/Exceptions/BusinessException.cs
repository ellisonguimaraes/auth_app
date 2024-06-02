using System.Diagnostics;

namespace AuthApp.Services.Exceptions;

public class BusinessException : Exception
{
    public string? TraceId { get; set; }

    public BusinessException(string? message) : base(message)
    {
        TraceId = Activity.Current?.Id;
    }

    public BusinessException(string? message, Exception? innerException) : base(message, innerException)
    {
        TraceId = Activity.Current?.Id;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, TraceId: {TraceId}";
    }
}
