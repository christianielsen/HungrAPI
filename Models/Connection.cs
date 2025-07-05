using HungrAPI.Enums;

namespace HungrAPI.Models;

public class Connection
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid User1Id { get; set; }
    
    public Guid User2Id { get; set; }
    
    public ConnectionStatus Status { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
}