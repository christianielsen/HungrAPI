using HungrAPI.Models;

namespace HungrAPI.Services.UserService;

public interface IUserService
{
    Task CreateUserAsync(User user);
}