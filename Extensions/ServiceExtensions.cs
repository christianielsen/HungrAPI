using System.Text;
using HungrAPI.Configuration;
using HungrAPI.Services.ConnectionService;
using HungrAPI.Services.PlacesService;
using HungrAPI.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace HungrAPI.Extensions;

public static class ServiceExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IConnectionService, ConnectionService>();
        services.AddScoped<IPlacesService, PlacesService>();
        services.AddScoped<IUserService, UserService>();
    }

    public static void AddJwtAuthentication(this IServiceCollection services, JwtConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Audience = configuration.Audience;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.FromSeconds(configuration.Clockskew),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Secret)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = configuration.Audience,
                ValidIssuer = configuration.Issuer
            };
        });
    }

    public static void AddGoogleHttpClient(this IServiceCollection services, GoogleConfiguration configuration)
    {
        services.AddHttpClient(Constants.HttpClient.AuthenticatedGoogleApiClient, client =>
        {
            client.BaseAddress = new Uri(configuration.Url);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
    }
}