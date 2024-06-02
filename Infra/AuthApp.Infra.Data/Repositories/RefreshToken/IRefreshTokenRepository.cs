namespace AuthApp.Infra.Data.Repositories.RefreshToken;

public interface IRefreshTokenRepository
{
    Task<IEnumerable<Domain.RefreshToken>> GetAllByUserIdAsync(string userId);

    Domain.RefreshToken? FindByToken(string token);

    Task<Domain.RefreshToken> CreateAsync(Domain.RefreshToken refreshToken);

    Task<Domain.RefreshToken> UpdateAsync(Domain.RefreshToken refreshToken);

    Task DeleteAsync(string id);
}
