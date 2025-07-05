using HungrAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HungrAPI.Data;

public class HungrDbContext(DbContextOptions<HungrDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Connection> Connections { get; set; }
}