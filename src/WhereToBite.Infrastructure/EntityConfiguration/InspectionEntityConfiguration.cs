using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Infrastructure.EntityConfiguration
{
    public class InspectionEntityConfiguration : IEntityTypeConfiguration<Inspection>
    {
        public void Configure(EntityTypeBuilder<Inspection> inspectionConfiguration)
        {
            inspectionConfiguration.ToTable("Inspection", WhereToBiteContext.DefaultSchema);
            
            inspectionConfiguration.HasKey(i => i.Id);
            
            inspectionConfiguration.Property<int>("_inspectionStatusId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("InspectionStatusId")
                .IsRequired();

            inspectionConfiguration.HasOne(x => x.InspectionStatus)
                .WithMany()
                .HasForeignKey("_inspectionStatusId");
            
            var navigation = inspectionConfiguration.Metadata.FindNavigation(nameof(Inspection.Infractions));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}