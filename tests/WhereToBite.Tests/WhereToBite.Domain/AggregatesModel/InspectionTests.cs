using System;
using System.Collections.Generic;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;
using Xunit;

namespace WhereToBite.Tests.WhereToBite.Domain.AggregatesModel
{
    public class InspectionTests
    {
        [Fact]
        public void ShouldCreateInstance()
        {
            var actual = new Inspection(InspectionStatus.Pass, DateTime.Now);
            Assert.NotNull(actual);
        }

        [Fact]
        public void ShouldAddNewInfraction()
        {
            var actual = new Inspection(InspectionStatus.Pass, DateTime.Now);

            var infractions = new[]
            {
                new Infraction(SeverityTypes.Minor, ActionTypes.Ticket, DateTime.Now, "", 0m)
            };
            
            actual.AddNewInfractions(infractions);
        }

        [Fact]
        public void ShouldOnlyAddNewInfractions()
        {
            var actual = new Inspection(InspectionStatus.Pass, DateTime.Now);

            var infractions = new List<Infraction>
            {
                new Infraction(SeverityTypes.Minor, ActionTypes.Ticket, DateTime.Now.AddHours(-2), "", 0m)
            };
            
            actual.AddNewInfractions(infractions);
            
            infractions.Add(new Infraction(SeverityTypes.Minor, ActionTypes.Ticket, DateTime.Now, "", 0m));
            
            actual.AddNewInfractions(infractions);
            
            Assert.Equal(2, actual.Infractions.Count);
            Assert.Equal(infractions, actual.Infractions);
        }
    }
}