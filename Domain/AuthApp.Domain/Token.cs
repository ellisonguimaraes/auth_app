using AuthApp.Domain.Enums;

namespace AuthApp.Domain;

public class Token
{
    public string? AccessToken { get; set; }

    public DateTime AccessTokenExpiresIn { get; set; }

    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpiresIn { get; set; }

    public TokenType TokenType { get; set; }
}
