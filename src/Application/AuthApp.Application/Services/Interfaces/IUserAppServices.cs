using AuthApp.Application.Models;
using AuthApp.Domain;
using AuthApp.Domain.Utils;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuthApp.Application.Services.Interfaces;

public interface IUserAppServices
{
    Task RegisterAsync(RegisterRequest registerRequest);
    Task<TokenResponse> AuthenticateAsync(AuthRequest authRequest);
    Task<TokenResponse> RefreshTokenAsync(string refreshToken);
    Task ConfirmEmailAsync(string email, string token);
    Task SendResetPasswordEmailAsync(string email);
    Task ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
    Task ChangePasswordAsync(string? sub, ChangePasswordRequest changePasswordRequest);
    Task<UserResponse> GetUserInfoAsync(string? sub);
    Task<PagedList<UserResponse>> GetLockoutUsersAsync(PaginationParameters paginationParameters);
    Task UnlockUserAsync(string id);
    Task AssignPersonToRegistrationAsync(string id, string personId);
}

