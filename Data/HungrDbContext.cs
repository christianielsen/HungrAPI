using Microsoft.EntityFrameworkCore;

namespace HungrAPI.Data;

public class HungrDbContext(DbContextOptions<HungrDbContext> options) : DbContext(options)
{
    
}