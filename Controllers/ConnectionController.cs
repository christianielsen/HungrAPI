using System.Security.Claims;
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
        var inviterEmail = User.FindFirst(ClaimTypes.Email)?.Value;
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

    [HttpPost("{connectionId:guid}/accept")]
    public async Task<IActionResult> Accept(Guid connectionId, [FromServices] IHubContext<ConnectionHub> hubContext)
    {
        var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(currentUserEmail))
            return Unauthorized();

        var currentUser = await _userService.GetUserByEmailAsync(currentUserEmail);
        if (currentUser == null)
        {
            return NotFound("Current user not found");
        }

        var connection = await _connectionService.GetConnectionAsync(connectionId);
        if (connection == null)
        {
            return NotFound("Connection not found");
        }

        if (connection.Status != ConnectionStatus.Pending)
        {
            return BadRequest("Connection status not pending");
        }

        await _connectionService.UpdateConnectionStatusAsync(connection, ConnectionStatus.Active);

        var payload = new
        {
            ConnectionId = connection.Id,
            acceptedBy = currentUser.Email
        };

        await hubContext.Clients.Group(connection.User1.Email).SendAsync("ConnectionAccepted", payload);
        await hubContext.Clients.Group(connection.User2.Email).SendAsync("ConnectionAccepted", payload);

        return NoContent();
    }
}