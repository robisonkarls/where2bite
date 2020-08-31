using System;
using System.Collections.Generic;
using System.Linq;
using WhereToBite.Domain.Exceptions;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public class InfractionAction : Enumeration
    {
        public static InfractionAction NoticeToComply = new InfractionAction(1, nameof(NoticeToComply).ToLowerInvariant());
        public static InfractionAction EducationProvided = new InfractionAction(2, nameof(EducationProvided).ToLowerInvariant());

        public static InfractionAction CorrectedDuringInspection =
            new InfractionAction(3, nameof(CorrectedDuringInspection).ToLowerInvariant());

        public static InfractionAction Summons = new InfractionAction(4, nameof(Summons).ToLowerInvariant());

        public static InfractionAction SummonsAndHealthHazardOrder =
            new InfractionAction(5, nameof(SummonsAndHealthHazardOrder).ToLowerInvariant());

        public static InfractionAction Ticket = new InfractionAction(6, nameof(Ticket).ToLowerInvariant());

        public InfractionAction(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<InfractionAction> List() => new[]
        {
            NoticeToComply, EducationProvided, CorrectedDuringInspection, Summons, SummonsAndHealthHazardOrder, Ticket
        };

        public static InfractionAction FromName(string name)
        {
            var cleanedName = name.Replace(" ", "");

            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, cleanedName, StringComparison.CurrentCultureIgnoreCase));

            return state ??
                   throw new WhereToBiteDomainException(
                       $"Possible values for Severity: {string.Join(",", List().Select(s => s.Name))}");
        }

        public static InfractionAction From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            return state ??
                   throw new WhereToBiteDomainException(
                       $"Possible values for Severity: {string.Join(",", List().Select(s => s.Name))}");
        }
    }
}