using System;
using System.Linq;
using NetTopologySuite.Geometries;
using WhereToBite.Api.Infrastructure.Mappers;
using WhereToBite.Api.Model;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;
using Xunit;

namespace WhereToBite.Tests.WhereToBite.Api.Infrastructure.Mappers
{
    public class DomainToResponseMapperTests
    {
        [Fact]
        public void ShouldMapEmptyLastInspectionIfThereIsZeroInspection()
        {
            // arrange
            var expectedPoint = new Point(1, 2);
            var expectedEstablishments = new[]
            {
                new Establishment(1, 
                    "TestName", 
                    "type", 
                    "", 
                    EstablishmentStatus.Closed.ToString(), 
                    expectedPoint)
            };

            var mapper = new DomainToResponseMapper();
            
            // act
            var actual = mapper.MapEstablishmentResponses(expectedEstablishments);

            // assert
            var actualMappedEstablishment = Assert.Single(actual);
            Assert.NotNull(actualMappedEstablishment);
            Assert.Equal(LastInspection.Empty.Status, actualMappedEstablishment.LastInspection.Status);
            Assert.Equal(expectedPoint.X, actualMappedEstablishment.Longitude);
            Assert.Equal(expectedPoint.Y, actualMappedEstablishment.Latitude);
        }
        
        [Fact]
        public void ShouldMapValuesForOverallAmountFinedAndOverAllInfractions()
        {
            // arrange
            var expectedPoint = new Point(1, 2);

            var expectedEstablishment = new Establishment(
                1,
                "TestName",
                "type",
                "",
                EstablishmentStatus.Closed.ToString(),
                expectedPoint);
            var expectedInspection = new Inspection(InspectionStatus.Pass.ToString(), DateTime.Today.AddDays(-10));

            var expectedInfraction = new Infraction(
                "C - Crucial", 
                "Ticket", 
                expectedInspection.Date, 
                "Fine",
                10000.00m);
            
            var expectedInfractionTwo = new Infraction(
                "M - Minor", 
                "Ticket", 
                expectedInspection.Date,
                "Fine", 
                10.00m);
            
            expectedInspection.AddNewInfractions(new [] { expectedInfraction, expectedInfractionTwo });
            
            expectedEstablishment.AddNewInspections(new[] { expectedInspection });
            
            var expectedEstablishments = new[]
            {
                expectedEstablishment
            };

            var mapper = new DomainToResponseMapper();
            
            // act
            var actual = mapper.MapEstablishmentResponses(expectedEstablishments);

            // assert
            var lastInspectionStatus = expectedEstablishment.GetLastInspection();
            var actualMappedEstablishment = Assert.Single(actual);
            Assert.NotNull(actualMappedEstablishment);
            Assert.Equal(lastInspectionStatus.InspectionStatus.Name, actualMappedEstablishment.LastInspection.Status);
            Assert.Equal(expectedPoint.X, actualMappedEstablishment.Longitude);
            Assert.Equal(expectedPoint.Y, actualMappedEstablishment.Latitude);
            Assert.Equal(expectedInfraction.AmountFined + expectedInfractionTwo.AmountFined, actualMappedEstablishment.OverallAmountFined);
            Assert.Equal(expectedInspection.Infractions.Count, actualMappedEstablishment.OverallNumberOfInfractions);
        }

        [Fact]
        public void ShouldMapInspectionResponse()
        {
            // assert
            var expectedInspection = new Inspection(InspectionStatus.Pass.ToString(), DateTime.Today.AddDays(-10));

            var expectedInfraction = new Infraction(
                "C - Crucial", 
                "Ticket", 
                expectedInspection.Date, 
                "Fine",
                10000.00m);
            
            var expectedInfractionTwo = new Infraction(
                "M - Minor", 
                "Ticket", 
                expectedInspection.Date,
                "Fine", 
                10.00m);
            
            expectedInspection.AddNewInfractions(new [] { expectedInfraction, expectedInfractionTwo });
            var mapper = new DomainToResponseMapper();
            
            // act
            var actual = mapper.MapInspectionResponses(new[] {expectedInspection});

            // assert
            var actualInspection = Assert.Single(actual);
            Assert.NotNull(actualInspection);
            Assert.Equal(expectedInspection.Date, actualInspection.Date);
            Assert.Equal(expectedInspection.InspectionStatus.Name, actualInspection.Status);
            Assert.Equal(expectedInspection.Infractions.Count, actualInspection.Infractions.Count());
        }
    }
}