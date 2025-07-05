using Microsoft.AspNetCore.SignalR;

namespace HungrAPI.Hubs;

public class ConnectionHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userEmail = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

        if (!string.IsNullOrEmpty(userEmail))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userEmail);
        }

        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userEmail = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

        if (!string.IsNullOrEmpty(userEmail))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userEmail);
        }

        await base.OnDisconnectedAsync(exception);
    }
}