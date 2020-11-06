using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Infrastructure.EntityConfiguration
{
    internal sealed class InfractionEntityConfiguration : IEntityTypeConfiguration<Infraction>
    {
        private const string SeverityFkName = "_severityId";
        private const string ActionFkName = "_infractionActionId";

        public void Configure(EntityTypeBuilder<Infraction> infractionConfiguration)
        {
            infractionConfiguration.ToTable("Infraction", WhereToBiteContext.DefaultSchema);
            
            infractionConfiguration.HasKey(i => i.Id);

            infractionConfiguration.Property(x => x.AmountFined);

            infractionConfiguration.Property(x => x.CourtOutcome)
                .IsRequired();

            infractionConfiguration.Property<int>(SeverityFkName)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("SeverityId")
                .IsRequired();

            infractionConfiguration.HasOne(x => x.Severity)
                .WithMany()
                .HasForeignKey(SeverityFkName);

            infractionConfiguration.Property<int>(ActionFkName)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("InfractionActionId")
                .IsRequired();

            infractionConfiguration.HasOne(x => x.InfractionAction)
                .WithMany()
                .HasForeignKey(ActionFkName);
        }
    }
}