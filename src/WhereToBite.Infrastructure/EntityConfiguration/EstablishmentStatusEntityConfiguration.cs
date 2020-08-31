using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Infrastructure.EntityConfiguration
{
    public class EstablishmentStatusEntityConfiguration : IEntityTypeConfiguration<EstablishmentStatus>
    {
        public void Configure(EntityTypeBuilder<EstablishmentStatus> establishmentStatusConfiguration)
        {
            establishmentStatusConfiguration.ToTable("EstablishmentStatus", WhereToBiteContext.DefaultSchema);
            
            establishmentStatusConfiguration.HasKey(x => x.Id);
            
            establishmentStatusConfiguration.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedNever();

            establishmentStatusConfiguration.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}