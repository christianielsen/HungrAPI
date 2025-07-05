using HungrAPI.Data;
using HungrAPI.Models;

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
}