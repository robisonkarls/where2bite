using System;
using JetBrains.Annotations;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public class Infraction : Entity
    {
        public Severity Severity => Severity.From(_severityId);
        
        [UsedImplicitly]
        private int _severityId;
        public InfractionAction InfractionAction => InfractionAction.From(_infractionActionId);
        
        [UsedImplicitly]
        private int _infractionActionId;
        public DateTime ConvictionDate { get; }
        public string CourtOutcome { get; }
        public decimal AmountFined { get; }

        public Infraction(
            string severity, 
            string infractionAction, 
            DateTime date, 
            string courtOutcome, 
            decimal amountFined) : this()
        {
            if (severity == null)
            {
                throw new ArgumentNullException(nameof(severity));
            }
            
            if (infractionAction == null)
            {
                throw new ArgumentNullException(nameof(infractionAction));
            }

            _severityId = Severity.FromName(severity).Id;
            _infractionActionId = InfractionAction.FromName(infractionAction).Id;
            ConvictionDate = date;
            CourtOutcome = courtOutcome ?? throw new ArgumentNullException(nameof(courtOutcome));
            AmountFined = amountFined;
        }
        
        protected Infraction() { }
    }
}