using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Infrastructure.EntityConfiguration
{
    internal sealed class EstablishmentEntityConfiguration : IEntityTypeConfiguration<Establishment>
    {
        private const string EstablishmentStatusFkName = "_establishmentStatusId";

        public void Configure(EntityTypeBuilder<Establishment> establishmentConfiguration)
        {
            establishmentConfiguration.ToTable("Establishments", WhereToBiteContext.DefaultSchema);
            
            establishmentConfiguration.HasKey(e => e.Id);
            
            establishmentConfiguration.Property(e => e.Latitude).IsRequired();
            
            establishmentConfiguration.Property(e => e.Longitude).IsRequired();
            
            establishmentConfiguration.Property(e => e.Address).IsRequired();
            
            establishmentConfiguration.Property(e => e.Name).IsRequired();
            
            establishmentConfiguration.Property(e => e.Type).IsRequired();
            
            establishmentConfiguration.Property(e => e.DineSafeId).IsRequired();

            establishmentConfiguration.Property(x => x.CreatedAt)
                .HasDefaultValueSql("now()");
            
            establishmentConfiguration.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("now()");

            establishmentConfiguration.Property(x => x.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate();
            
            establishmentConfiguration.Property(x => x.UpdatedAt)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            
            establishmentConfiguration
                .HasIndex(e => e.DineSafeId)
                .IsUnique();

            establishmentConfiguration
                .Property<int>(EstablishmentStatusFkName)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("EstablishmentStatusId")
                .IsRequired();

            var navigation = establishmentConfiguration.Metadata.FindNavigation(nameof(Establishment.Inspections));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            establishmentConfiguration
                .HasOne(x => x.EstablishmentStatus)
                .WithMany()
                .HasForeignKey(EstablishmentStatusFkName);
        }
    }
}