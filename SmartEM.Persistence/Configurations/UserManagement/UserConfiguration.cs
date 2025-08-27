using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Persistence.Configurations.UserManagement;

public class UserConfiguration : BaseEntityConfiguration<User, Guid>, IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(SmartEMDbContext.Users), PersistenceConstants.IDENTITY_SCHEMA);
        ConfigureBase(builder);
        builder.Property(u => u.Email).HasMaxLength(100).IsRequired();
        builder.Property(u => u.IsFirstLogin);
        builder.Property(u => u.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(50).IsRequired();
        builder.Property(u => u.JobTitle).HasMaxLength(100);
        builder.Property(u => u.LastLoginTimeStamp);
        builder.Property(u => u.LastPasswordResetToken);
        builder.Property(u => u.PasswordHash).HasMaxLength(256);
        builder.Property(u => u.AccountType).HasConversion<string>();
        builder.OwnsOne(u => u.Photo, n =>
        {
            n.Property(p => p.FileName).HasMaxLength(256);
            n.Property(p => p.ContentType).HasMaxLength(50);
        });
        builder.HasMany(u => u.ExternalLogins).WithOne(e => e.User)
                                              .HasForeignKey(e => e.UserId)
                                              .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.RefreshTokens).WithOne(r => r.User)
                                           .HasForeignKey(r => r.UserId)
                                           .OnDelete(DeleteBehavior.Cascade);

    }
}
