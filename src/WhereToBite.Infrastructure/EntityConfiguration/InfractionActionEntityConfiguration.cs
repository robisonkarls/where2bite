using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Infrastructure.EntityConfiguration
{
    internal sealed class InfractionActionEntityConfiguration : IEntityTypeConfiguration<InfractionAction>
    {
        public void Configure(EntityTypeBuilder<InfractionAction> infractionActionConfiguration)
        {
            infractionActionConfiguration.ToTable("InfractionAction", WhereToBiteContext.DefaultSchema);
            
            infractionActionConfiguration.HasKey(x => x.Id);
            
            infractionActionConfiguration.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedNever();

            infractionActionConfiguration.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}