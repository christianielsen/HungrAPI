using HungrAPI.Enums;
using HungrAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HungrAPI.Services.ConnectionService;

public interface IConnectionService
{
    Task AddConnectionAsync(Connection connection);
    Task<Connection?> GetConnectionAsync(Guid connectionId);
    Task UpdateConnectionStatusAsync(Connection connection, ConnectionStatus status);
}