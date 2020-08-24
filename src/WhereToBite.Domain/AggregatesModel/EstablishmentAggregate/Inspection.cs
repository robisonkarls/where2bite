using System;
using System.Collections.Generic;
using System.Linq;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public class Inspection : Entity
    {
        public IReadOnlyCollection<Infraction> Infractions => _infractions;
        private readonly List<Infraction> _infractions;
        
        private int? _establishmentId;
        public InspectionStatus Status { get; }
        public DateTime Date { get; }

        public Inspection(InspectionStatus status, DateTime date) : this()
        {
            Status = status ?? throw new ArgumentNullException(nameof(status));
            Date = date;
        }

        protected Inspection()
        {
            _infractions = new List<Infraction>();
        }

        public void AddNewInfractions(IEnumerable<Infraction> infractions)
        {
            var infractionsList = infractions.ToList();
            
            if (_infractions.Count < infractionsList.Count)
            {
                var lastInfractionDate = _infractions
                    .Select(i => i.Date)
                    .OrderByDescending(i => i.Date)
                    .FirstOrDefault();
                
                var filteredInspection = infractionsList.Where(i => i.Date > lastInfractionDate);
                _infractions.AddRange(filteredInspection);
            }
            
            
        }
    }
}