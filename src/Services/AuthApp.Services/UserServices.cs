using System.Linq.Expressions;
using AuthApp.Domain;
using AuthApp.Domain.Email;
using AuthApp.Domain.Enums;
using AuthApp.Domain.Utils;
using AuthApp.Infra.CrossCutting.Resources;
using AuthApp.Infra.Data.Repositories.RefreshToken;
using AuthApp.Services.Exceptions;
using AuthApp.Services.Extensions;
using AuthApp.Services.Interfaces;
using AuthApp.Services.Utils.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace AuthApp.Services;

public class UserServices : IUserServices
{
    #region Constants
    private const string URL_BASE_PROPERTY_NAME = "UrlBase";
    #endregion

    private readonly UserManager<User> _userManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtServices _jwtServices;
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;
    private readonly string _urlBase;

    public UserServices(UserManager<User> userManager, IRefreshTokenRepository refreshTokenRepository, IJwtServices jwtServices, IEmailSender emailSender, IConfiguration configuration)
    {
        _userManager = userManager;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtServices = jwtServices;
        _emailSender = emailSender;
        _configuration = configuration;
        _urlBase = $"{_configuration[URL_BASE_PROPERTY_NAME]}";
    }

    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="user">User</param>
    /// <param name="password">Password</param>
    public async Task RegisterAsync(User user, string password, bool isLockout = false)
    {
        if (UserExists(user))
            throw new AuthException(ErrorCodeResource.REGISTER_USER_ALREADY_EXISTS);

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new AuthException(ErrorCodeResource.REGISTER_COULD_NOT_REGISTER_USER, result);

        await AddToRoleAsync(user);


        await _userManager.SetLockoutEnabledAsync(user, isLockout);

        try
        {
            var message = await BuildConfirmationEmailAsync(user);
            await _emailSender.SendAsync(message);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    /// <summary>
    /// User exists verify
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>User exists or not</returns>
    private bool UserExists(User user) => _userManager.Users.Any(u =>
            u.UserName.Equals(user.UserName) ||
            u.Email.Equals(user.Email));

    /// <summary>
    /// Person id exists verify
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>PersonId exists or not</returns>
    public bool PersonIdExists(Guid personId) => _userManager.Users.Any(u =>
            u.PersonId != null && u.PersonId.Equals(personId));

    /// <summary>
    /// User with this document exists verify
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>Exists or not</returns>
    public bool UserWithThisDocumentExists(string document, DocumentType documentType) => _userManager.Users.Any(u =>
            u.DocumentType.Equals(documentType) && u.Document.Equals(document));

    /// <summary>
    /// Build confirmation email message
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>MimeMessage</returns>
    private async Task<MimeMessage> BuildConfirmationEmailAsync(User user)
    {
        var emailToken = (await _userManager.GenerateEmailConfirmationTokenAsync(user)).Base64Encode();

        var url = $"{_urlBase}/confirmar-email?token={emailToken}&email={user.Email}";

        var to = new List<To> { new To(user.Name, user.Email) };
        var body = string.Format(TemplatesResource.EMAIL_CONFIRM_MESSAGE, user.Name, url);
        var message = new Message(to, TemplatesResource.EMAIL_CONFIRM_SUBJECT, body);

        return _emailSender.CreateEmailMessage(message);
    }

    /// <summary>
    /// Add user role
    /// </summary>
    /// <param name="user">User</param>
    private async Task AddToRoleAsync(User user)
    {
        var getUser = await _userManager.FindByEmailAsync(user.Email);

        if (getUser is null)
            throw new AuthException(ErrorCodeResource.USER_NOT_FOUND);

        string permission = string.Empty;

        switch (getUser.UserType)
        {
            case UserType.STUDENT:
                permission = PermissionType.STUDENT.ToString();
                break;
            case UserType.EGRESS:
                permission = PermissionType.EGRESS.ToString();
                break;
            case UserType.TEACHER:
                permission = PermissionType.TEACHER.ToString();
                break;
        }

        var result = await _userManager.AddToRoleAsync(getUser, permission);

        if (!result.Succeeded)
            throw new AuthException(ErrorCodeResource.COULD_NOT_DEFINE_ROLE);
    }

    /// <summary>
    /// Authenticate User
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="password">Password</param>
    /// <returns>Token Payload</returns>
    public async Task<Token> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            throw new AuthException(ErrorCodeResource.USER_NOT_FOUND);

        if (await IsLockedAsync(user))
            throw new AuthException(ErrorCodeResource.USER_IS_BLOCKED);

        if (!await _userManager.CheckPasswordAsync(user, password))
        {
            await _userManager.AccessFailedAsync(user);
            throw new AuthException(ErrorCodeResource.INVALID_CREDENTIALS);
        }

        if (!await _userManager.IsEmailConfirmedAsync(user))
            throw new AuthException(ErrorCodeResource.CONFIRM_YOUR_ACCOUNT);

        await _userManager.ResetAccessFailedCountAsync(user);

        var token = await GenerateTokenAsync(user);

        return token;
    }

    /// <summary>
    /// Check if user is locked
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>True or false</returns>
    private async Task<bool> IsLockedAsync(User user)
    {
        var lockoutEnabled = await _userManager.GetLockoutEnabledAsync(user);
        var lockoutEndDate = await _userManager.GetLockoutEndDateAsync(user);

        return lockoutEndDate is null ? lockoutEnabled : lockoutEnabled || ((DateTimeOffset)lockoutEndDate).Ticks > DateTime.UtcNow.Ticks;
    }

    /// <summary>
    /// Generete user token
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>Token Payload</returns>
    private async Task<Token> GenerateTokenAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtServices.GenerateToken(user, roles.First());

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = token.RefreshToken,
            ExpiresIn = token.RefreshTokenExpiresIn,
            CreatedAt = DateTime.UtcNow,
            EditedAt = DateTime.UtcNow,
            IsActive = true,
            User = user
        };

        await _refreshTokenRepository.CreateAsync(refreshTokenEntity);

        return token;
    }

    /// <summary>
    /// Generate new token through RefreshToken
    /// </summary>
    /// <param name="refreshToken">RefreshToken</param>
    /// <returns>Token Payload</returns>
    public async Task<Token> RefreshTokenAsync(string refreshToken)
    {
        var refreshTokenEntity = _refreshTokenRepository.FindByToken(refreshToken);

        if (refreshTokenEntity is null || refreshTokenEntity.User is null)
            throw new AuthException(ErrorCodeResource.INVALID_REFRESH_TOKEN);

        if (await IsLockedAsync(refreshTokenEntity.User))
            throw new AuthException(ErrorCodeResource.USER_IS_BLOCKED);

        if (!refreshTokenEntity.IsActive || refreshTokenEntity.ExpiresIn < DateTime.UtcNow)
            throw new AuthException(ErrorCodeResource.EXPIRED_REFRESH_TOKEN);

        await RevokeRefreshTokenAsync(refreshTokenEntity);

        var token = await GenerateTokenAsync(refreshTokenEntity.User);

        return token;
    }

    /// <summary>
    /// Revoke token: set IsActive = false
    /// </summary>
    /// <param name="refreshToken">Refresh Token Entity</param>
    private async Task RevokeRefreshTokenAsync(RefreshToken refreshToken)
    {
        refreshToken.IsActive = false;
        await _refreshTokenRepository.UpdateAsync(refreshToken);
    }

    /// <summary>
    /// Confirm user email
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="token">Confirmation token</param>
    public async Task ConfirmEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            throw new AuthException(ErrorCodeResource.USER_NOT_FOUND);

        var result = await _userManager.ConfirmEmailAsync(user, token.Base64Decode());

        if (!result.Succeeded)
            throw new AuthException(ErrorCodeResource.COULD_NOT_CONFIRM, result);
    }

    /// <summary>
    /// Reset user password
    /// </summary>
    /// <param name="email">Email</param>
    public async Task SendResetPasswordEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            throw new AuthException(ErrorCodeResource.USER_NOT_FOUND);

        var message = await BuildResetPasswordEmailAsync(user);

        await _emailSender.SendAsync(message);
    }

    /// <summary>
    /// Build reset password email
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>MimeMessage</returns>
    private async Task<MimeMessage> BuildResetPasswordEmailAsync(User user)
    {
        var emailToken = (await _userManager.GeneratePasswordResetTokenAsync(user)).Base64Encode();

        var url = $"{_urlBase}/recuperar-senha/nova-senha?token={emailToken}&email={user.Email}";

        var to = new List<To> { new To(user.Name, user.Email) };
        var body = string.Format(TemplatesResource.EMAIL_RESET_PASSWORD_MESSAGE, user.Name, url);
        var message = new Message(to, TemplatesResource.EMAIL_RESET_PASSWORD_SUBJECT, body);

        return _emailSender.CreateEmailMessage(message);
    }

    /// <summary>
    /// Reset user password
    /// </summary>
    /// <param name="email">Email</param>
    public async Task ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            throw new AuthException(ErrorCodeResource.USER_NOT_FOUND);

        var result = await _userManager.ResetPasswordAsync(user, token.Base64Decode(), newPassword);

        if (!result.Succeeded)
            throw new AuthException(ErrorCodeResource.COULD_NOT_RESET_PASSWORD);

        await RevokeAllRefreshTokenAsync(user.Id);

        var message = BuildResetPasswordConfirmedEmail(user);

        await _emailSender.SendAsync(message);
    }

    /// <summary>
    /// Revoke all refresh token
    /// </summary>
    /// <param name="user">User</param>
    private async Task RevokeAllRefreshTokenAsync(string userId)
    {
        var refreshTokens = await _refreshTokenRepository.GetAllByUserIdAsync(userId);

        foreach (var refreshToken in refreshTokens)
            await RevokeRefreshTokenAsync(refreshToken);
    }

    /// <summary>
    /// Build reset password confirm email
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>MimeMessage</returns>
    private MimeMessage BuildResetPasswordConfirmedEmail(User user)
    {
        var to = new List<To> { new To(user.Name, user.Email) };
        var body = string.Format(TemplatesResource.CONFIRMED_EMAIL_RESET_PASSWORD_MESSAGE, user.Name, _urlBase);
        var message = new Message(to, TemplatesResource.CONFIRMED_EMAIL_RESET_PASSWORD_SUBJECT, body);

        return _emailSender.CreateEmailMessage(message);
    }

    /// <summary>
    /// Change user password
    /// </summary>
    /// <param name="id">User id</param>
    /// <param name="password">Password</param>
    /// <param name="newPassword">New password</param>
    public async Task ChangePasswordAsync(string id, string password, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            throw new AuthException(ErrorCodeResource.USER_NOT_FOUND);

        if (!await _userManager.CheckPasswordAsync(user, password))
            throw new AuthException(ErrorCodeResource.INVALID_PASSWORD);

        var result = await _userManager.ChangePasswordAsync(user, password, newPassword);

        if (!result.Succeeded)
            throw new AuthException(ErrorCodeResource.COULD_NOT_CHANGE_PASSWORD);

        await RevokeAllRefreshTokenAsync(user.Id);
    }

    /// <summary>
    /// Get user info
    /// </summary>
    /// <param name="sub">User id</param>
    /// <returns>User</returns>
    public async Task<(User, IList<string>)> GetUserInfoAsync(string sub)
    {
        var user = await _userManager.FindByIdAsync(sub);

        if (user is null)
            throw new AuthException(ErrorCodeResource.USER_NOT_FOUND);

        var roles = await _userManager.GetRolesAsync(user);

        return (user, roles);
    }

    /// <summary>
    /// Get paginate user
    /// </summary>
    /// <param name="paginationParameters">Page number and page size</param>
    /// <param name="orderByPropertySeletor">Order by property seletor</param>
    /// <param name="predicate">Filter/predicate</param>
    /// <returns>Paginate users</returns>
    public async Task<PagedList<User>> GetPaginateUsersAsync(PaginationParameters paginationParameters, Expression<Func<User, string>> orderByPropertySeletor, Expression<Func<User, bool>>? predicate = null)
    {
        var users = new PagedList<User>(
            predicate is null
                ? _userManager.Users
                    .OrderBy(orderByPropertySeletor)
                : _userManager.Users
                    .OrderBy(orderByPropertySeletor)
                    .Where(predicate),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);

        foreach (var user in users)
            user.Roles = await _userManager.GetRolesAsync(user);

        return users;
    }

    /// <summary>
    /// Unlock user
    /// </summary>
    /// <param name="id">Id</param>
    public async Task UnlockUserAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null)
            throw new AuthException(ErrorCodeResource.USER_NOT_FOUND);

        user.LockoutEnabled = false;
        user.LockoutEnd = null;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new AuthException(ErrorCodeResource.COULD_NOT_UNLOCK_USER);
    }
    
    /// <summary>
    /// Assign person to registration
    /// </summary>
    /// <param name="id">User id</param>
    /// <param name="personId">Person id</param>
    public async Task AssignPersonToRegistrationAsync(Guid id, Guid personId)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null)
            throw new AuthException(ErrorCodeResource.USER_NOT_FOUND);

        user.PersonId = personId;
        user.HasCompleteRegister = true;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new AuthException(ErrorCodeResource.COULD_NOT_ASSIGN_PERSON_TO_REGISTRATION);
    }

    /// <summary>
    /// Get user roles
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>Roles list</returns>
    public async Task<IList<string>> GetRolesAsync(User user)
        => await _userManager.GetRolesAsync(user);
}
