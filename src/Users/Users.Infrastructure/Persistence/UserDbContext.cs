using Microsoft.EntityFrameworkCore;
using Users.Domain;

namespace Users.Infrastructure.Persistence;

public class UserDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("Users");
        builder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
        base.OnModelCreating(builder);
    }
}