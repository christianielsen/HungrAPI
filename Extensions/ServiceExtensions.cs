using System.Text;
using HungrAPI.Configuration;
using HungrAPI.Services.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace HungrAPI.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
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


}