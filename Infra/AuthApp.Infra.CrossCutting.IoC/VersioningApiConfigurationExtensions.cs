using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthApp.Infra.CrossCutting.IoC;

/// <summary>
/// IServiceCollection extensions to use API Versioning 
/// </summary>
public static class VersioningApiConfigurationExtensions
{
    #region CONSTANTS
    private const string HEADER_API_VERSION = "X-Version";
    private const string QUERY_STRING_API_VERSION = "api-version";
    private const string MEDIA_TYPE_API_VERSION = "ver";
    private const string FORMAT_API_VERSION = "'v'VVV";
    private const bool SUBSTITUTE_API_VERSION_IN_URL = true;
    private const string API_DEFAULT_VERSION_PROPERTY = "ApiDefaultVersion";
    private const bool REPORT_API_VERSIONS = true;
    private const bool ASSUME_DEFAULT_VERSION_WHEN_UNSPECIFIED = true;
    private const string DOT = ".";
    #endregion

    /// <summary>
    /// Allows the use of versioning in the API
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="configuration">Configuration file (appsettings)</param>
    public static void AddApiVersioningConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultVersion = configuration[API_DEFAULT_VERSION_PROPERTY].Split(DOT);
        var majorVersion = int.Parse(defaultVersion.First());
        var minorVersion = int.Parse(defaultVersion.Last());

        services.AddApiVersioning(o =>
        {
            o.AssumeDefaultVersionWhenUnspecified = ASSUME_DEFAULT_VERSION_WHEN_UNSPECIFIED;
            o.DefaultApiVersion = new ApiVersion(majorVersion, minorVersion);
            o.ReportApiVersions = REPORT_API_VERSIONS;
            o.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader(QUERY_STRING_API_VERSION),
                new HeaderApiVersionReader(HEADER_API_VERSION),
                new MediaTypeApiVersionReader(MEDIA_TYPE_API_VERSION));
        });

        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = FORMAT_API_VERSION;
            setup.SubstituteApiVersionInUrl = SUBSTITUTE_API_VERSION_IN_URL;
        });
    }
}
