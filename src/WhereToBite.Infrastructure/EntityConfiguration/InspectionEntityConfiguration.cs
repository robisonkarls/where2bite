using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Infrastructure.EntityConfiguration
{
    internal sealed class InspectionEntityConfiguration : IEntityTypeConfiguration<Inspection>
    {
        public void Configure(EntityTypeBuilder<Inspection> inspectionConfiguration)
        {
            inspectionConfiguration.ToTable("Inspection", WhereToBiteContext.DefaultSchema);
            
            inspectionConfiguration.HasKey(i => i.Id);

            inspectionConfiguration.Property(x => x.Date)
                .IsRequired();
            
            inspectionConfiguration.Property<int>("_inspectionStatusId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("InspectionStatusId")
                .IsRequired();
            
            inspectionConfiguration
                .HasMany(x => x.Infractions)
                .WithOne();

            var navigation = inspectionConfiguration.Metadata.FindNavigation(nameof(Inspection.Infractions));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}