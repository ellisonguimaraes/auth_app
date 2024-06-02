using AuthApp.Application;
using AuthApp.Application.Mapper;
using AuthApp.Application.Models;
using AuthApp.Application.Services;
using AuthApp.Application.Services.Interfaces;
using AuthApp.Application.Validators;
using AuthApp.Domain.Settings;
using AuthApp.Infra.Data.Repositories.RefreshToken;
using AuthApp.Services;
using AuthApp.Services.HttpClients;
using AuthApp.Services.Interfaces;
using AuthApp.Services.Utils.EmailSender;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace AuthApp.Infra.CrossCutting.IoC;

/// <summary>
/// IServiceCollection extension to register services
/// </summary>
public static class DependencyInjectionExtensions
{
    private const string EMAIL_SETTINGS_PROPERTY_NAME = "EmailSettings";
    private const string EGRESS_API_SETTINGS_PROPERTY_NAME = "EgressApiSettings";
    private const string EGRESS_API_SETTINGS_BASE_URL = "BaseUrl";
    private const string EGRESS_API_SETTINGS_VERSION = "Version";

    /// <summary>
    /// Register services
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="configuration">Configuration file (appsettings)</param>
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Settings
        services.Configure<EmailSettings>(configuration.GetSection(EMAIL_SETTINGS_PROPERTY_NAME));

        // Services
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IJwtServices, JwtServices>();
        services.AddScoped<IUserServices, UserServices>();

        // App Services
        services.AddScoped<IUserAppServices, UserAppServices>();
        services.AddScoped<IContactAppServices, ContactAppServices>();

        // Repositories
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        // Validators
        services.AddScoped<IValidator<AuthRequest>, AuthRequestValidator>();
        services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
        services.AddScoped<IValidator<ResetPasswordRequest>, ResetPasswordRequestValidator>();
        services.AddScoped<IValidator<ChangePasswordRequest>, ChangePasswordRequestValidator>();
        services.AddScoped<IValidator<ContactEmailRequest>, ContactEmailRequestValidator>();

        // Mapper
        services.AddAutoMapper(typeof(TokenProfile));
        services.AddAutoMapper(typeof(UserProfile));

        // HttpClient Configurations
        var baseUrl = $"http://{configuration[$"{EGRESS_API_SETTINGS_PROPERTY_NAME}:{EGRESS_API_SETTINGS_BASE_URL}"]}/{configuration[$"{EGRESS_API_SETTINGS_PROPERTY_NAME}:{EGRESS_API_SETTINGS_VERSION}"]}";
        services.AddRefitClient<IEgressHttpClient>()
            .ConfigureHttpClient(cfg => cfg.BaseAddress = new Uri(baseUrl));
    }
}
