using AuthApp.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AuthApp.Infra.CrossCutting.IoC;

/// <summary>
/// Web Application Extensions
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Create roles in database
    /// </summary>
    /// <param name="app">WebApplication</param>
    public static async Task CreateRolesInDbAsync(this WebApplication app)
    {
        var permissionTypeList = Enum.GetNames<PermissionType>();

        using var scope = app.Services.CreateScope();

        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

        foreach (var permissionType in permissionTypeList)
            if (!await roleManager!.RoleExistsAsync(permissionType))
                await roleManager.CreateAsync(new IdentityRole(permissionType.ToString()));
    }
}
