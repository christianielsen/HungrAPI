using HungrAPI.Services.AuthService.Dtos;

namespace HungrAPI.Services.AuthService;

public interface IAuthService
{
    Task<bool> GoogleLogin(GoogleLoginDto loginDto);
}