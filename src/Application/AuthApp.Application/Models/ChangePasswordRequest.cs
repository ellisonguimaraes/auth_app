using System.Text.Json.Serialization;

namespace AuthApp.Application.Models;

public class ChangePasswordRequest
{
    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("new_password")]
    public string NewPassword { get; set; }

    [JsonPropertyName("new_password_repeat")]
    public string NewPasswordRepeat { get; set; }
}
