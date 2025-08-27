using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEM.Domain;

namespace SmartEM.Persistence.Configurations;

public class BaseEntityConfiguration<TEntity, TId>
    where TEntity : BaseEntity<TId>
    where TId : IComparable<TId>
{
    protected void ConfigureBase(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.SequentialId).UseSequence();
    }
}
