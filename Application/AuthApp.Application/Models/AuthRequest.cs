using System.Text.Json.Serialization;

namespace AuthApp.Application.Models;

public class AuthRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}
