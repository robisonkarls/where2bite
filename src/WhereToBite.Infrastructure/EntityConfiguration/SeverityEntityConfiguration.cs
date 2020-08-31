using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Infrastructure.EntityConfiguration
{
    public class SeverityEntityConfiguration : IEntityTypeConfiguration<Severity>
    {
        public void Configure(EntityTypeBuilder<Severity> severityConfiguration)
        {
            severityConfiguration.ToTable("Severity", WhereToBiteContext.DefaultSchema);
            
            severityConfiguration.HasKey(x => x.Id);
            
            severityConfiguration.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedNever();

            severityConfiguration.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}