using HungrAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HungrAPI.Data;

public class HungrDbContext(DbContextOptions<HungrDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Connection> Connections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Connection>()
            .HasOne(c => c.User1)
            .WithMany()
            .HasForeignKey(c => c.User1Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Connection>()
            .HasOne(c => c.User2)
            .WithMany()
            .HasForeignKey(c => c.User2Id)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}