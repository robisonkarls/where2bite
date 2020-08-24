using System;
using System.Collections.Generic;
using System.Linq;
using WhereToBite.Domain.Exceptions;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public class ActionTypes : Enumeration
    {
        public static ActionTypes NoticeToComply = new ActionTypes(1, nameof(NoticeToComply).ToLowerInvariant());
        public static ActionTypes EducationProvided = new ActionTypes(2, nameof(EducationProvided).ToLowerInvariant());

        public static ActionTypes CorrectedDuringInspection =
            new ActionTypes(3, nameof(CorrectedDuringInspection).ToLowerInvariant());

        public static ActionTypes Summons = new ActionTypes(4, nameof(Summons).ToLowerInvariant());

        public static ActionTypes SummonsAndHealthHazardOrder =
            new ActionTypes(5, nameof(SummonsAndHealthHazardOrder).ToLowerInvariant());

        public static ActionTypes Ticket = new ActionTypes(6, nameof(Ticket).ToLowerInvariant());

        public ActionTypes(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<ActionTypes> List() => new[]
        {
            NoticeToComply, EducationProvided, CorrectedDuringInspection, Summons, SummonsAndHealthHazardOrder, Ticket
        };

        public static ActionTypes FromName(string name)
        {
            var cleanedName = name.Replace(" ", "");

            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, cleanedName, StringComparison.CurrentCultureIgnoreCase));

            return state ??
                   throw new WhereToBiteDomainException(
                       $"Possible values for Severity: {string.Join(",", List().Select(s => s.Name))}");
        }

        public static ActionTypes From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            return state ??
                   throw new WhereToBiteDomainException(
                       $"Possible values for Severity: {string.Join(",", List().Select(s => s.Name))}");
        }
    }
}