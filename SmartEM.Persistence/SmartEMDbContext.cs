using Microsoft.EntityFrameworkCore;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Persistence;

public class SmartEMDbContext(DbContextOptions<SmartEMDbContext> options) : DbContext(options)
{
    //user management entities
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ExternalLogin> ExternalLogins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartEMDbContext).Assembly);
    }

    /// <summary>
    /// Here, we have configured 256 as the default maximum length for all string properties in the domain.
    /// This safely overrides the default nvarchar(max), which inhibits performance.
    /// Configure actual numbers where you're sure of the maximum length of the string.
    /// For indexed strings, override to a maximum value of 450 to prevent SQL Server errors.
    /// For large text (e.g., JSON, HTML) you may override to nvarchar(max) or a large number to avoid truncation.
    /// </summary>
    /// <param name="configurationBuilder"></param>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>()
            .HaveMaxLength(256);
    }
}
