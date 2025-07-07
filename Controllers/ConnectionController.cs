using System.Security.Claims;
using HungrAPI.Constants;
using HungrAPI.Enums;
using HungrAPI.Hubs;
using HungrAPI.Models;
using HungrAPI.Services.ConnectionService;
using HungrAPI.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HungrAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ConnectionController(IConnectionService connectionService, IUserService userService) : ControllerBase
{
    private readonly IConnectionService _connectionService = connectionService;
    private readonly IUserService _userService = userService;

    [HttpPost("[action]")]
    public async Task<IActionResult> Invite(InviteDto dto, [FromServices] IHubContext<ConnectionHub> hubContext)
    {
        var inviterEmail = User.FindFirst(JwtClaims.Email)?.Value;
        if (string.IsNullOrEmpty(inviterEmail))
        {
            return Unauthorized();
        }

        var inviter = await _userService.GetUserByEmailAsync(inviterEmail);
        if (inviter == null)
        {
            return NotFound("Inviter not found");
        }

        var invitee = await _userService.GetUserByEmailAsync(dto.Email);
        if (invitee == null)
        {
            return NotFound("Invitee not found");
        }

        var connection = new Connection
        {
            User1Id = inviter.Id,
            User2Id = invitee.Id,
            Status = ConnectionStatus.Pending,
            CreatedAt = DateTimeOffset.Now.ToUniversalTime()
        };
        await _connectionService.AddConnectionAsync(connection);

        await hubContext.Clients.Groups(dto.Email).SendAsync("ReceiveInvite", new
        {
            ConnectionId = connection.Id,
            From = inviterEmail
        });

        return Ok(new { connection.Id });
    }
}