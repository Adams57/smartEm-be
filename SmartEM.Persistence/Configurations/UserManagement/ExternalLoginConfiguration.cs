using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Persistence.Configurations.UserManagement;

public class ExternalLoginConfiguration : BaseEntityConfiguration<ExternalLogin, Guid>, IEntityTypeConfiguration<ExternalLogin>
{
    public void Configure(EntityTypeBuilder<ExternalLogin> builder)
    {
        builder.ToTable(nameof(SmartEMDbContext.ExternalLogins), PersistenceConstants.IDENTITY_SCHEMA);
        ConfigureBase(builder);
        builder.Property(e => e.ProviderId).IsRequired();
        builder.Property(e => e.ProviderName).HasConversion<string>()
                                             .IsRequired();
        builder.HasIndex(e => e.UserId);
    }
}
