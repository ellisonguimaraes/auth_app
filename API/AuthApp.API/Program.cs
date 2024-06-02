using AuthApp.API.Middlewares;
using AuthApp.Infra.CrossCutting.IoC;
using AuthApp.Infra.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Serilog;

#region CONSTANTS
const string APPLICATION_PROPERTY = "Application";
const string APPLICATION_NAME = "ApplicationName";
const string AUTHDB_PROPERTY = "AuthDbConnectionString";
const string HEALTH_CHECK_ROUTE = "/health";
#endregion

var builder = WebApplication.CreateBuilder(args);

#region Configure Services
//Serilog Configuration
builder.Logging.ClearProviders();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty(APPLICATION_PROPERTY, builder.Configuration[APPLICATION_NAME])
    .CreateLogger();

builder.Logging.AddSerilog(logger);

// Database Context Configuration
// Migration command: dotnet ef migrations add InitialMigrations -p .\Infra\AuthApp.Infra.Data\ -s .\API\AuthApp.API\
// Update Db command: dotnet ef database update -p .\Infra\AuthApp.Infra.Data\ -s .\API\AuthApp.API\
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration.GetConnectionString(AUTHDB_PROPERTY));

// Identity Configuration
builder.Services.AddIdentityConfiguration(builder.Configuration);

// API Versioning
builder.Services.AddApiVersioningConfiguration(builder.Configuration);

// Health Check Configuration
builder.Services.AddHealthChecks();

// Clear .NET Built-in Validator (in the action inside the controller)
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


// Add services to the container.
builder.Services.AddControllers();
builder.Services.RegisterServices(builder.Configuration);
#endregion

var app = builder.Build();

app.UseHttpsRedirection();

app.UseMiddleware<PerformanceMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks(HEALTH_CHECK_ROUTE);

app.MapControllers();

await app.CreateRolesInDbAsync();

app.Run();

