using System.Text.Json.Serialization;

namespace AuthApp.Domain.Settings;

/// <summary>
/// Jwt Settings in appsettings.json
/// </summary>
public class JwtSettings
{
    [JsonPropertyName("SecretKey")]
    public string? SecretKey { get; set; }

    [JsonPropertyName("Audience")]
    public string? Audience { get; set; }

    [JsonPropertyName("Issuer")]
    public string? Issuer { get; set; }

    [JsonPropertyName("ExpiryTimeInSeconds")]
    public long ExpiryTimeInSeconds { get; set; }
    
    [JsonPropertyName("RefreshTokenExpiryTimeInSeconds")]
    public long RefreshTokenExpiryTimeInSeconds { get; set; }

    [JsonPropertyName("ValidateActor")]
    public bool ValidateActor { get; set; }

    [JsonPropertyName("ValidateAudience")]
    public bool ValidateAudience { get; set; }

    [JsonPropertyName("ValidateLifetime")]
    public bool ValidateLifetime { get; set; }

    [JsonPropertyName("ValidateIssuerSigningKey")]
    public bool ValidateIssuerSigningKey { get; set; }
}
