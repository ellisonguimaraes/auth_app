using AuthApp.Domain.Enums;
using System.Text.Json.Serialization;

namespace AuthApp.Application.Models;

public class RegisterRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("confirm_password")]
    public string PasswordRepeat { get; set; }

    [JsonPropertyName("document")]
    public string Document { get; set; }

    [JsonPropertyName("document_type")]
    public DocumentType DocumentType { get; set; }

    [JsonPropertyName("user_type")]
    public UserType UserType { get; set; }
}
