using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public class Inspection : Entity
    {
        public IReadOnlyCollection<Infraction> Infractions => _infractions;
        
        private readonly List<Infraction> _infractions;
        
        public InspectionStatus InspectionStatus => InspectionStatus.From(_inspectionStatusId);
        
        [UsedImplicitly]
        private int _inspectionStatusId;
        public DateTime Date { get; }

        public Inspection(string inspectionStatus, DateTime date) : this()
        {
            _inspectionStatusId = InspectionStatus.FromName(inspectionStatus).Id;
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