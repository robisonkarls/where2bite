using System;
using System.Collections.Generic;
using System.Linq;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public class Establishment : Entity, IAggregateRoot
    {
        public IReadOnlyCollection<Inspection> Inspections => _inspections;
        private readonly List<Inspection> _inspections;
        public int DineSafeId { get; }
        public string Name { get; }
        public string Type { get; }
        public string Address { get; }
        public string Longitude { get; }
        public string Latitude { get; }
        public EstablishmentStatus Status { get; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        public Establishment(int dineSafeId, string name, string type, string address, string longitude,
            string latitude, EstablishmentStatus status) : this()
        {
            DineSafeId = dineSafeId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Longitude = longitude ?? throw new ArgumentNullException(nameof(longitude));
            Latitude = latitude ?? throw new ArgumentNullException(nameof(latitude));
            Status = status ?? throw new ArgumentNullException(nameof(status));
        }

        protected Establishment()
        {
            _inspections = new List<Inspection>();
        }

        public void AddNewInspections(IEnumerable<Inspection> inspections)
        {
            if (inspections == null) throw new ArgumentNullException(nameof(inspections));
        
            var inspectionList = inspections.ToList();
            
            if (_inspections.Count <= inspectionList.Count)
            {
                var lastInspectionDate = _inspections
                    .Select(i => i.Date)
                    .OrderByDescending(i => i.Date)
                    .FirstOrDefault();
                
                var filteredInspection = inspectionList.Where(i => i.Date > lastInspectionDate);
                _inspections.AddRange(filteredInspection);
            }
        }
    }
}