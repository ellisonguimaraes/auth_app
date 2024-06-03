using AuthApp.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace AuthApp.Domain;

/// <summary>
/// User Entity
/// </summary>
public class User : IdentityUser
{
    public string Name { get; set; }

    public string Document { get; set; }

    public Guid? PersonId { get; set; }

    public DocumentType DocumentType { get; set; }

    public UserType UserType { get; set; }

    public bool HasCompleteRegister { get; set; } = false;

    public DateTime CreatedAt { get; set; }

    public DateTime EditedAt { get; set; }

    // Relationship
    public virtual List<RefreshToken> RefreshTokens { get; set; }
    
    // Ignore 
    public IList<string> Roles { get; set; }
}
