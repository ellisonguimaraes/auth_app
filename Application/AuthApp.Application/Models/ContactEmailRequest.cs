using System.Text.Json.Serialization;

namespace AuthApp.Application;

public class ContactEmailRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("subject")]
    public string Subject { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}
