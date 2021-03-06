﻿using System;
using System.Collections.Generic;
using System.Linq;
using WhereToBite.Domain.Exceptions;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public class InfractionAction : Enumeration
    {
        public static InfractionAction NoticeToComply = new(1, nameof(NoticeToComply).ToLowerInvariant());
        public static InfractionAction EducationProvided = new(2, nameof(EducationProvided).ToLowerInvariant());
        public static InfractionAction CorrectedDuringInspection =
            new(3, nameof(CorrectedDuringInspection).ToLowerInvariant());

        public static InfractionAction Summons = new(4, nameof(Summons).ToLowerInvariant());
        public static InfractionAction SummonsAndHealthHazardOrder =
            new(5, nameof(SummonsAndHealthHazardOrder).ToLowerInvariant());

        public static InfractionAction Ticket = new(6, nameof(Ticket).ToLowerInvariant());
        public static InfractionAction NotInCompliance = new(7, nameof(NotInCompliance).ToLowerInvariant());
        public static InfractionAction SummonsByLaw = new(8, nameof(SummonsByLaw).ToLowerInvariant());
        public static InfractionAction ClosureOrder = new(9, nameof(ClosureOrder).ToLowerInvariant());

        public InfractionAction(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<InfractionAction> List() => new[]
        {
            NoticeToComply, 
            EducationProvided, 
            CorrectedDuringInspection, 
            Summons, 
            SummonsAndHealthHazardOrder, 
            Ticket, 
            NotInCompliance, 
            SummonsByLaw,
            ClosureOrder
        };

        public static InfractionAction FromName(string name)
        {
            var cleanedName = name.Replace(" ", "");

            cleanedName = cleanedName.Replace("-", ""); 

            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, cleanedName, StringComparison.CurrentCultureIgnoreCase));

            return state ??
                   throw new WhereToBiteDomainException(
                       $"Possible values for InfractionAction: {string.Join(",", List().Select(s => s.Name))}");
        }

        public static InfractionAction From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            return state ??
                   throw new WhereToBiteDomainException(
                       $"Possible values for InfractionAction: {string.Join(",", List().Select(s => s.Name))}");
        }
    }
}