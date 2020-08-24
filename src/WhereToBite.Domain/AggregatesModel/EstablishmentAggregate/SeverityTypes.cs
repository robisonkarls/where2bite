using System;
using System.Collections.Generic;
using System.Linq;
using WhereToBite.Domain.Exceptions;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public class SeverityTypes : Enumeration
    {
        public static SeverityTypes Minor = new SeverityTypes(1, nameof(Minor).ToLowerInvariant());
        public static SeverityTypes Significant = new SeverityTypes(2, nameof(Significant).ToLowerInvariant());
        public static SeverityTypes Crucial = new SeverityTypes(3, nameof(Crucial).ToLowerInvariant());
        public static SeverityTypes NotApplicable = new SeverityTypes(4, nameof(NotApplicable).ToLowerInvariant());

        public SeverityTypes(int id, string name) : base(id, name)
        {
        }
        
        public static IEnumerable<SeverityTypes> List() => new[] { Minor, Significant, Crucial, NotApplicable };

        public static SeverityTypes FromName(string name)
        {
            var nameSplit = name.Split(" ");

            if (nameSplit.Length != 3)
            {
                throw new InvalidOperationException("Invalid data for Severity");
            }

            var cleanedName = nameSplit[2].Replace(" ", "");
            
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, cleanedName, StringComparison.CurrentCultureIgnoreCase));
            
            return state ?? throw new WhereToBiteDomainException($"Possible values for Severity: {string.Join(",", List().Select(s => s.Name))}");
        }

        public static SeverityTypes From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);
            
            return state ?? throw new WhereToBiteDomainException($"Possible values for Severity: {string.Join(",", List().Select(s => s.Name))}");
        }
    }
}