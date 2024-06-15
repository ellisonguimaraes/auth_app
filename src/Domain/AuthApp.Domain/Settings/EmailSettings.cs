using System.Text.Json.Serialization;

namespace AuthApp.Domain.Settings;

public class EmailSettings
{
    [JsonPropertyName("From")]
    public string From { get; set; }

    [JsonPropertyName("DisplayName")]
    public string DisplayName { get; set; }

    [JsonPropertyName("UserName")]
    public string UserName { get; set; }

    [JsonPropertyName("Password")]
    public string Password { get; set; }

    [JsonPropertyName("Smtp")]
    public string Smtp { get; set; }

    [JsonPropertyName("Port")]
    public int Port { get; set; }

    [JsonPropertyName("UseSsl")]
    public bool UseSsl { get; set; }

    [JsonPropertyName("TimeoutSmtpOperationsInMilliseconds")]
    public int TimeoutSmtpOperationsInMilliseconds { get; set; }
}
