using System;
using System.Collections.Generic;
using System.Linq;
using WhereToBite.Domain.Exceptions;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public class InspectionStatus : Enumeration
    {
        public static InspectionStatus Pass = new InspectionStatus(1, nameof(Pass).ToLowerInvariant());
        public static InspectionStatus ConditionalPass = new InspectionStatus(2, nameof(ConditionalPass).ToLowerInvariant());
        public static InspectionStatus Closed = new InspectionStatus(3, nameof(Closed).ToLowerInvariant());
        
        public InspectionStatus(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<InspectionStatus> List() => new[] { Pass, ConditionalPass, Closed };

        public static InspectionStatus FromName(string name)
        {
            var transformedName = name.Replace(" ", "");
            
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, transformedName, StringComparison.CurrentCultureIgnoreCase));
            
            return state ?? throw new WhereToBiteDomainException($"Possible values for EstablishmentStatus: {string.Join(",", List().Select(s => s.Name))}");
        }

        public static InspectionStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);
            
            return state ?? throw new WhereToBiteDomainException($"Possible values for EstablishmentStatus: {string.Join(",", List().Select(s => s.Name))}");
        }
    }
}