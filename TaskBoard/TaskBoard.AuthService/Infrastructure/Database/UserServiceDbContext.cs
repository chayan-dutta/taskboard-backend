using Microsoft.EntityFrameworkCore;
using TaskBoard.AuthService.Infrastructure.Entities;

namespace TaskBoard.AuthService.Infrastructure.Database;

public class UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Username).IsUnique();

            // JSONB mapping
            entity.Property(u => u.Roles)
                  .HasColumnType("jsonb")
                  .HasConversion(
                      v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                      v => System.Text.Json.JsonSerializer.Deserialize<List<Role>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<Role>()
                  );
        });
    }
}
