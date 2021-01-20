using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NetTopologySuite.Geometries;
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
        public Point Location { get; }
        public string Latitude { get; }
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public EstablishmentStatus EstablishmentStatus { get; }
        
        [UsedImplicitly]
        private int _establishmentStatusId;
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        public Establishment(int dineSafeId, 
            string name, 
            string type, 
            string address, 
            string longitude,
            string latitude, 
            string establishmentStatus,
            [NotNull] Point location) : this()
        {
            _establishmentStatusId = EstablishmentStatus.FromName(establishmentStatus).Id;
            DineSafeId = dineSafeId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Longitude = longitude ?? throw new ArgumentNullException(nameof(longitude));
            Latitude = latitude ?? throw new ArgumentNullException(nameof(latitude));
            Location = location ?? throw new ArgumentNullException(nameof(location));
        }

        protected Establishment()
        {
            _inspections = new List<Inspection>();
        }

        public void AddNewInspections(IEnumerable<Inspection> inspections)
        {
            if (inspections == null) throw new ArgumentNullException(nameof(inspections));
        
            var inspectionList = inspections.ToList();
            
            if (_inspections.Count < inspectionList.Count)
            {
                if (_inspections.Any())
                {
                    var lastInspectionDate = _inspections.Max(x => x.Date);
                    
                    var filteredInspection = inspectionList.Where(i => i.Date > lastInspectionDate);
                    
                    _inspections.AddRange(filteredInspection);
                }
                else
                {
                    _inspections.AddRange(inspectionList);
                }
            }
        }
    }
}