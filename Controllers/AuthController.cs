using HungrAPI.Services.AuthService;
using HungrAPI.Services.AuthService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HungrAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login/google")]
    public async Task<IActionResult> GoogleLogin(GoogleLoginDto loginDto)
    {
        var result = await _authService.GoogleLogin(loginDto);
        
        return result ? Ok() : Unauthorized();
    }
}