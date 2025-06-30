using System.Data;
using Google.Apis.Auth;
using HungrAPI.Data;
using HungrAPI.Models;
using HungrAPI.Services.AuthService.Dtos;
using Microsoft.EntityFrameworkCore;

namespace HungrAPI.Services.AuthService;

public class AuthService(HungrDbContext dbContext) : IAuthService
{
    private readonly HungrDbContext _dbContext = dbContext;
    
    public async Task<bool> GoogleLogin(GoogleLoginDto loginDto)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(loginDto.IdToken);

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.GoogleId == payload.Subject);

            if (user == null)
            {
                user = new User
                {
                    GoogleId = payload.Subject,
                    Name = payload.Name,
                    Email = payload.Email,
                    AvatarUrl = payload.Picture
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }

            return true;
        }
        catch (InvalidJwtException)
        {
            await transaction.RollbackAsync();
            return false;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Failed to login user");
        }
    }
}