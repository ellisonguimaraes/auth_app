using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace AuthApp.Services.Exceptions;

public class AuthException : Exception
{
    public string? TraceId { get; set; }

    public IdentityResult? IdentityResult { get; set; }

    public AuthException(string? message) : base(message)
    {
        TraceId = Activity.Current?.Id;
    }

    public AuthException(string? message, Exception? innerException) : base(message, innerException)
    {
        TraceId = Activity.Current?.Id;
    }

    public AuthException(string? message, IdentityResult identityResult) : this(message)
    {
        IdentityResult = identityResult;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, TraceId: {TraceId}";
    }
}
