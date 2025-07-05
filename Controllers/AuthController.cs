using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HungrAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    [HttpGet("test")]
    public async Task<IActionResult> GoogleLogin()
    {
        Console.WriteLine(HttpContext.Request.Headers.Authorization.ToString());
        return Ok(new { hello = "hello world" });
    }
}