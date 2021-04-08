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
            var actual = new Inspection("Pass", DateTime.Now);
            Assert.NotNull(actual);
        }

        [Fact]
        public void ShouldAddNewInfraction()
        {
            var actual = new Inspection("Pass", DateTime.Now);

            var infractions = new[]
            {
                new Infraction("M - Minor", "Ticket", DateTime.Now, "", 0m)
            };
            
            actual.AddNewInfractions(infractions);
        }

        [Fact]
        public void ShouldOnlyAddNewInfractions()
        {
            var actual = new Inspection("Pass", DateTime.Now);

            var infractions = new List<Infraction>
            {
                new Infraction("M - Minor", "Ticket", DateTime.Now.AddHours(-2), "", 0m)
            };
            
            actual.AddNewInfractions(infractions);
            
            infractions.Add(new Infraction("M - Minor", "Ticket", DateTime.Now, "", 0m));
            
            actual.AddNewInfractions(infractions);
            
            Assert.Equal(2, actual.Infractions.Count);
            Assert.Equal(infractions, actual.Infractions);
        }
    }
}