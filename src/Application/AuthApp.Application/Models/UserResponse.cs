using AuthApp.Domain.Enums;
using System.Text.Json.Serialization;

namespace AuthApp.Application.Models;

public class UserResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("document")]
    public string Document { get; set; }

    [JsonPropertyName("document_type")]
    public DocumentType DocumentType { get; set; }

    [JsonPropertyName("has_complete_register")]
    public bool HasCompleteRegister { get; set; }

    [JsonPropertyName("person_id")]
    public Guid? PersonId { get; set; }

    [JsonPropertyName("user_type")]
    public UserType UserType { get; set; }

    [JsonPropertyName("email_confirmed")]
    public bool EmailConfirmed { get; set; }

    [JsonPropertyName("lockout_end")]
    public DateTimeOffset? LockoutEnd { get; set; }

    [JsonPropertyName("lockout_enabled")]
    public bool LockoutEnabled { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("edited_at")]
    public DateTime EditedAt { get; set; }

    [JsonPropertyName("roles")]
    public IEnumerable<string> Roles { get; set; }
}
