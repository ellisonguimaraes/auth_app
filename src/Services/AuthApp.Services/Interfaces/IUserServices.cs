using System.Linq.Expressions;
using AuthApp.Domain;
using AuthApp.Domain.Enums;
using AuthApp.Domain.Utils;

namespace AuthApp.Services.Interfaces;

public interface IUserServices
{
    Task RegisterAsync(User user, string password, bool isLockout = false);

    Task<Token> AuthenticateAsync(string email, string password);

    Task<Token> RefreshTokenAsync(string refreshToken);

    Task ConfirmEmailAsync(string email, string token);

    Task SendResetPasswordEmailAsync(string email);

    Task ResetPasswordAsync(string email, string token, string newPassword);

    Task<(User, IList<string>)> GetUserInfoAsync(string sub);

    Task ChangePasswordAsync(string id, string password, string newPassword);

    bool PersonIdExists(Guid personId);

    bool UserWithThisDocumentExists(string document, DocumentType documentType);

    Task<PagedList<User>> GetPaginateUsersAsync(PaginationParameters paginationParameters, Expression<Func<User, string>> orderByPropertySeletor, Expression<Func<User, bool>>? predicate = null);

    Task UnlockUserAsync(Guid id);

    Task AssignPersonToRegistrationAsync(Guid id, Guid personId);

    Task<IList<string>> GetRolesAsync(User user);
}
