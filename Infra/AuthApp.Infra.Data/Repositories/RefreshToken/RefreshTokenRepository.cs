using AuthApp.Infra.CrossCutting.Resources;
using AuthApp.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthApp.Infra.Data.Repositories.RefreshToken;

/// <summary>
/// Refresh token repository class
/// </summary>
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<RefreshTokenRepository> _logger;

    public RefreshTokenRepository(ApplicationDbContext applicationDbContext, ILogger<RefreshTokenRepository> logger)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
    }

    /// <summary>
    /// Get all refresh tokens
    /// </summary>
    /// <returns>All refresh tokens</returns>
    public async Task<IEnumerable<Domain.RefreshToken>> GetAllByUserIdAsync(string userId)
        => await _applicationDbContext.RefreshTokens
            .Where(x => x.UserId.Equals(userId))
            .ToListAsync();

    /// <summary>
    /// Find refresh token by token
    /// </summary>
    /// <param name="token">Token string</param>
    /// <returns>Refresh token entity</returns>
    public Domain.RefreshToken? FindByToken(string token)
        => _applicationDbContext.RefreshTokens
            .Include(rt => rt.User)
            .SingleOrDefault(rt => rt.Token.Equals(token));

    /// <summary>
    /// Save refresh token in database 
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <returns>Refresh token with id</returns>
    public async Task<Domain.RefreshToken> CreateAsync(Domain.RefreshToken refreshToken)
    {
        _applicationDbContext.RefreshTokens.Add(refreshToken);
        await _applicationDbContext.SaveChangesAsync();
        return refreshToken;
    }

    /// <summary>
    /// Update refresh token in database
    /// </summary>
    /// <param name="refreshToken">Refresh token entity</param>
    /// <returns>Refresh token updated</returns>
    public async Task<Domain.RefreshToken> UpdateAsync(Domain.RefreshToken refreshToken)
    {
        var refreshTokenEntity = _applicationDbContext.RefreshTokens.SingleOrDefault(rt => rt.Id.Equals(refreshToken.Id));

        if (refreshTokenEntity is null)
            throw new ArgumentNullException(nameof(refreshToken), ErrorCodeResource.DB_COULD_NOT_GET_REFRESHTOKEN_TO_UPDATE);

        _applicationDbContext.Entry(refreshTokenEntity).CurrentValues.SetValues(refreshToken);

        await _applicationDbContext.SaveChangesAsync();

        return refreshTokenEntity;
    }

    /// <summary>
    /// Delete refresh token in database
    /// </summary>
    /// <param name="id">Refresh token id</param>
    public async Task DeleteAsync(string id)
    {
        var refreshToken = _applicationDbContext.RefreshTokens.SingleOrDefault(rt => rt.Id.Equals(id));

        if (refreshToken is null)
            throw new ArgumentNullException(nameof(refreshToken), ErrorCodeResource.DB_COULD_NOT_GET_REFRESHTOKEN_TO_DELETE);

        _applicationDbContext.RefreshTokens.Remove(refreshToken);

        await _applicationDbContext.SaveChangesAsync();
    }
}
