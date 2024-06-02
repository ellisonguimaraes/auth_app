namespace AuthApp.Domain;

public class RefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; }

    public DateTime ExpiresIn { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime EditedAt { get; set; }

    // Relationship
    public string UserId { get; set; }
    public virtual User User { get; set; }
}
