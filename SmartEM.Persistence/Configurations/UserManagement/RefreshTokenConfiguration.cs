using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Persistence.Configurations.UserManagement;

public class RefreshTokenConfiguration : BaseEntityConfiguration<RefreshToken, Guid>, IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable(nameof(SmartEMDbContext.RefreshTokens), PersistenceConstants.IDENTITY_SCHEMA);
        ConfigureBase(builder);
        builder.Property(r => r.IsRevoked).HasDefaultValue(false);
        builder.Property(r => r.TokenString).IsRequired();
        builder.Property(r => r.Expiry).IsRequired();
        builder.HasIndex(r => r.UserId);
    }
}
