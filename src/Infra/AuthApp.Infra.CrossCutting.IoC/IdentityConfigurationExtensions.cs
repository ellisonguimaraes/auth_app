using AuthApp.Domain;
using AuthApp.Domain.Enums;
using AuthApp.Domain.Settings;
using AuthApp.Infra.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace AuthApp.Infra.CrossCutting.IoC;

/// <summary>
/// IServiceCollection extensions to use Identity 
/// </summary>
public static class IdentityConfigurationExtensions
{
    #region CONSTANTS
    private const string JWT_SETTINGS_PROPERTY = "JwtSettings";

    private const string IDENTITY_SETTINGS_PASSWORD_REQUIRENONALPHANUMERIC_PROPERTY = "IdentitySetting:Password:RequireNonAlphanumeric";
    private const string IDENTITY_SETTINGS_PASSWORD_REQUIREDIGIT_PROPERTY = "IdentitySetting:Password:RequireDigit";
    private const string IDENTITY_SETTINGS_PASSWORD_REQUIREUPPERCASE_PROPERTY = "IdentitySetting:Password:RequireUppercase";
    private const string IDENTITY_SETTINGS_PASSWORD_REQUIRELOWERCASE_PROPERTY = "IdentitySetting:Password:RequireLowercase";
    private const string IDENTITY_SETTINGS_PASSWORD_REQUIRELENGTH_PROPERTY = "IdentitySetting:Password:RequiredLength";

    private const string IDENTITY_SETTINGS_SIGNIN_REQUIREUNIQUEEMAIL_PROPERTY = "IdentitySetting:SignIn:RequireUniqueEmail";

    private const string IDENTITY_SETTINGS_USER_REQUIRECONFIRMEDEMAIL_PROPERTY = "IdentitySetting:User:RequireConfirmedEmail";

    private const string IDENTITY_SETTINGS_LOCKOUT_ALLOWEDFORNEWUSERS_PROPERTY = "IdentitySetting:Lockout:AllowedForNewUsers";
    private const string IDENTITY_SETTINGS_LOCKOUT_DEFAULTLOCKOUTINSECONDS_PROPERTY = "IdentitySetting:Lockout:DefaultLockoutInSeconds";
    private const string IDENTITY_SETTINGS_LOCKOUT_MAXFAILEDACCESSATTEMPTS_PROPERTY = "IdentitySetting:Lockout:MaxFailedAccessAttempts";
    #endregion
    
    /// <summary>
    /// Allows the use of Identity in the API
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="configuration">Configuration file (appsettings)</param>
    public static void AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(JWT_SETTINGS_PROPERTY).Get<JwtSettings>();

        services.AddSingleton(jwtSettings);

        // Identity settings
        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Lockout = new LockoutOptions
            {
                AllowedForNewUsers = bool.Parse(configuration[IDENTITY_SETTINGS_LOCKOUT_ALLOWEDFORNEWUSERS_PROPERTY]),
                DefaultLockoutTimeSpan = TimeSpan.FromSeconds(int.Parse(configuration[IDENTITY_SETTINGS_LOCKOUT_DEFAULTLOCKOUTINSECONDS_PROPERTY])),
                MaxFailedAccessAttempts = int.Parse(configuration[IDENTITY_SETTINGS_LOCKOUT_MAXFAILEDACCESSATTEMPTS_PROPERTY])
            };
            options.SignIn.RequireConfirmedEmail = bool.Parse(configuration[IDENTITY_SETTINGS_SIGNIN_REQUIREUNIQUEEMAIL_PROPERTY]);
            options.User.RequireUniqueEmail = bool.Parse(configuration[IDENTITY_SETTINGS_USER_REQUIRECONFIRMEDEMAIL_PROPERTY]);
            options.Password.RequireNonAlphanumeric = bool.Parse(configuration[IDENTITY_SETTINGS_PASSWORD_REQUIRENONALPHANUMERIC_PROPERTY]);
            options.Password.RequireDigit = bool.Parse(configuration[IDENTITY_SETTINGS_PASSWORD_REQUIREDIGIT_PROPERTY]);
            options.Password.RequireUppercase = bool.Parse(configuration[IDENTITY_SETTINGS_PASSWORD_REQUIREUPPERCASE_PROPERTY]);
            options.Password.RequireLowercase = bool.Parse(configuration[IDENTITY_SETTINGS_PASSWORD_REQUIRELOWERCASE_PROPERTY]);
            options.Password.RequiredLength = int.Parse(configuration[IDENTITY_SETTINGS_PASSWORD_REQUIRELENGTH_PROPERTY]);
        }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateActor = jwtSettings.ValidateActor,
                ValidateAudience = jwtSettings.ValidateAudience,
                ValidateLifetime = jwtSettings.ValidateLifetime,
                ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey!))
            };
        });

        // Authentication and Authorization settings
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PermissionType.ADM.ToString(), policy => policy.RequireAuthenticatedUser().RequireRole(PermissionType.ADM.ToString()));
        });
    }
}
