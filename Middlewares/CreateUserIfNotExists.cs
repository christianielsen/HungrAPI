using HungrAPI.Constants;
using HungrAPI.Data;
using HungrAPI.Extensions;
using HungrAPI.Services.UserService;

namespace HungrAPI.Middlewares;

public class CreateUserIfNotExists(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context, HungrDbContext dbContext, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrEmpty(token))
        {
            var tokenUser = token.GetUser();
            if (tokenUser is null)
            {
                return;
            }

            var existingUser = dbContext.Users.Any(u => u.Email == tokenUser.Email);

            if (!existingUser)
            {
                await userService.CreateUserAsync(tokenUser);
            }
        }

        await _next(context);
    }
}