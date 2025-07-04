using HungrAPI.Configuration;
using HungrAPI.Data;
using HungrAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtConfiguration = builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>();

if (jwtConfiguration is null)
{
    throw new Exception("JWT configuration not found");
}

if (connectionString is null)
{
    throw new Exception("Could not get connection string");
}

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.RegisterServices();
builder.Services.AddJwtAuthentication(jwtConfiguration);

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

app.MapControllers();

app.Run();