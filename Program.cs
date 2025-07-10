using HungrAPI.Configuration;
using HungrAPI.Data;
using HungrAPI.Extensions;
using HungrAPI.Hubs;
using HungrAPI.Middlewares;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtConfiguration = builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>();
var googleApiConfiguration = builder.Configuration.GetSection("Google").Get<GoogleConfiguration>();

if (jwtConfiguration is null || googleApiConfiguration is null)
{
    throw new Exception("Configurations not set correctly");
}

if (connectionString is null)
{
    throw new Exception("Could not get connection string");
}

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.RegisterServices();
builder.Services.AddJwtAuthentication(jwtConfiguration);
builder.Services.AddSignalR();
builder.Services.AddGoogleHttpClient(googleApiConfiguration);

builder.Services.AddDbContextPool<HungrDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<CreateUserIfNotExists>();

app.MapControllers();

app.MapHub<ConnectionHub>("/hubs/connection");
app.Run();