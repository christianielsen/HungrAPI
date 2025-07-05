using HungrAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HungrAPI.Services.ConnectionService;

public interface IConnectionService
{
    Task AddConnectionAsync(Connection connection);
}