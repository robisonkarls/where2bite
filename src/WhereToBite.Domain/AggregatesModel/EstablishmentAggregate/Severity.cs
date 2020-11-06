using System;
using System.Collections.Generic;
using System.Linq;
using WhereToBite.Domain.Exceptions;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public class Severity : Enumeration
    {
        public static Severity Minor = new Severity(1, nameof(Minor).ToLowerInvariant());
        public static Severity Significant = new Severity(2, nameof(Significant).ToLowerInvariant());
        public static Severity Crucial = new Severity(3, nameof(Crucial).ToLowerInvariant());
        public static Severity NotApplicable = new Severity(4, nameof(NotApplicable).ToLowerInvariant());

        public Severity(int id, string name) : base(id, name)
        {
        }
        
        public static IEnumerable<Severity> List() => new[] { Minor, Significant, Crucial, NotApplicable };

        public static Severity FromName(string name)
        {
            var nameSplit = name.Split("-");

            if (nameSplit.Length != 2)
            {
                throw new InvalidOperationException("Invalid data for Severity");
            }

            var cleanedName = nameSplit[1].Replace(" ", "");
            
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, cleanedName, StringComparison.CurrentCultureIgnoreCase));
            
            return state ?? throw new WhereToBiteDomainException($"Possible values for Severity: {string.Join(",", List().Select(s => s.Name))}");
        }

        public static Severity From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);
            
            return state ?? throw new WhereToBiteDomainException($"Possible values for Severity: {string.Join(",", List().Select(s => s.Name))}");
        }
    }
}