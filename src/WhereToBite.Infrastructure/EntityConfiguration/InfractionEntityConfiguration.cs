using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Infrastructure.EntityConfiguration
{
    public class InfractionEntityConfiguration : IEntityTypeConfiguration<Infraction>
    {
        public void Configure(EntityTypeBuilder<Infraction> infractionConfiguration)
        {
            infractionConfiguration.HasKey(i => i.Id);
            infractionConfiguration.Property(i => i.Action).IsRequired();
            infractionConfiguration.Property(i => i.Severity).IsRequired();
            infractionConfiguration.Property<int>("InspectionId").IsRequired();
        }
    }
}