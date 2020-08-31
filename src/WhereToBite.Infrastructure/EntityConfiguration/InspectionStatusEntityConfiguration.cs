using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Infrastructure.EntityConfiguration
{
    public class InspectionStatusEntityConfiguration : IEntityTypeConfiguration<InspectionStatus>
    {
       
        public void Configure(EntityTypeBuilder<InspectionStatus> inspectionStatusConfiguration)
        {
            inspectionStatusConfiguration.ToTable("InspectionStatus", WhereToBiteContext.DefaultSchema);
            
            inspectionStatusConfiguration.HasKey(x => x.Id);
            
            inspectionStatusConfiguration.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedNever();

            inspectionStatusConfiguration.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}