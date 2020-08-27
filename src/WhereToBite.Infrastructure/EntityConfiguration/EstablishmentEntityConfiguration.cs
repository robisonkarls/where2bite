using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Infrastructure.EntityConfiguration
{
    public class EstablishmentEntityConfiguration : IEntityTypeConfiguration<Establishment>
    {
        public void Configure(EntityTypeBuilder<Establishment> establishmentConfiguration)
        {
            establishmentConfiguration.HasKey(e => e.Id);
            establishmentConfiguration.Property(e => e.Latitude).IsRequired();
            establishmentConfiguration.Property(e => e.Longitude).IsRequired();
            establishmentConfiguration.Property(e => e.Address).IsRequired();
            establishmentConfiguration.Property(e => e.Name).IsRequired();
            establishmentConfiguration.Property(e => e.Status).IsRequired();
            establishmentConfiguration.Property(e => e.Type).IsRequired();
            establishmentConfiguration
                .HasIndex(e => e.DineSafeId)
                .IsUnique();
            
            var navigation = establishmentConfiguration.Metadata.FindNavigation(nameof(Establishment.Inspections));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}