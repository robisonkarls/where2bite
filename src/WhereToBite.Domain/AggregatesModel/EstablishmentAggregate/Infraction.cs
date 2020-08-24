using System;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public class Infraction : Entity
    {
        public SeverityTypes Severity { get; }
        public ActionTypes Action { get; }
        public DateTime Date { get; }
        public string CourtOutcome { get; }
        public decimal AmountFined { get; }

        public Infraction(SeverityTypes severity, ActionTypes action, DateTime date, string courtOutcome, decimal amountFined)
        {
            Severity = severity ?? throw new ArgumentNullException(nameof(severity));
            Action = action ?? throw new ArgumentNullException(nameof(action));
            Date = date;
            CourtOutcome = courtOutcome ?? throw new ArgumentNullException(nameof(courtOutcome));
            AmountFined = amountFined;
        }
    }
}