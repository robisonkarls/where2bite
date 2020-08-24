using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Infrastructure.EntityConfiguration
{
    public class InspectionEntityConfiguration : IEntityTypeConfiguration<Inspection>
    {
        public void Configure(EntityTypeBuilder<Inspection> inspectionConfiguration)
        {
            inspectionConfiguration.HasKey(i => i.Id);
            inspectionConfiguration.Property(i => i.Status).IsRequired();
            inspectionConfiguration.Property<int>("OrderId").IsRequired();

            
            var navigation = inspectionConfiguration.Metadata.FindNavigation(nameof(Inspection.Infractions));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}