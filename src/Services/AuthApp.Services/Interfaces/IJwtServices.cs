using AuthApp.Domain;
using AuthApp.Domain.Enums;

namespace AuthApp.Services.Interfaces;

public interface IJwtServices
{
    string ValidateJwtToken(string token);

    Token GenerateToken(User user, string role);
}
