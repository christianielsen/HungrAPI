using Google.Apis.Auth;
using HungrAPI.Data;
using HungrAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HungrAPI.Services.UserService;

public class UserService(HungrDbContext dbContext) : IUserService
{
    private readonly HungrDbContext _dbContext = dbContext;

    public async Task CreateUserAsync(User user)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            user.CreatedAt = DateTimeOffset.Now.ToUniversalTime();
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (InvalidJwtException)
        {
            await transaction.RollbackAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Failed to create user");
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}