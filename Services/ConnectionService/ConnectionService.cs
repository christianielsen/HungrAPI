using HungrAPI.Data;
using HungrAPI.Enums;
using HungrAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HungrAPI.Services.ConnectionService;

public class ConnectionService(HungrDbContext dbContext) : IConnectionService
{
    private readonly HungrDbContext _dbContext = dbContext;

    public async Task AddConnectionAsync(Connection connection)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            _dbContext.Connections.Add(connection);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Connection?> GetConnectionAsync(Guid connectionId)
    {
        return await _dbContext.Connections
            .Include(c => c.User1)
            .Include(c => c.User2)
            .FirstOrDefaultAsync(c => c.Id == connectionId);
    }

    public async Task UpdateConnectionStatusAsync(Connection connection, ConnectionStatus status)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            connection.Status = status;

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}