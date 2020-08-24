﻿using System;
using System.Collections.Generic;
using System.Linq;
using WhereToBite.Domain.Exceptions;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public class EstablishmentStatus : Enumeration
    {
        public static EstablishmentStatus Pass = new EstablishmentStatus(1, nameof(Pass).ToLowerInvariant());
        public static EstablishmentStatus ConditionalPass = new EstablishmentStatus(2, nameof(ConditionalPass).ToLowerInvariant());
        public static EstablishmentStatus Closed = new EstablishmentStatus(3, nameof(Closed).ToLowerInvariant());
        
        public EstablishmentStatus(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<EstablishmentStatus> List() => new[] { Pass, ConditionalPass, Closed };

        public static EstablishmentStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
            
            return state ?? throw new WhereToBiteDomainException($"Possible values for EstablishmentStatus: {string.Join(",", List().Select(s => s.Name))}");
        }

        public static EstablishmentStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);
            
            return state ?? throw new WhereToBiteDomainException($"Possible values for EstablishmentStatus: {string.Join(",", List().Select(s => s.Name))}");
        }
    }
}