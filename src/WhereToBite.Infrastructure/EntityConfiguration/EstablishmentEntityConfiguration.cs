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
            
            var navigation = establishmentConfiguration.Metadata.FindNavigation(nameof(Establishment.Inspections));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}