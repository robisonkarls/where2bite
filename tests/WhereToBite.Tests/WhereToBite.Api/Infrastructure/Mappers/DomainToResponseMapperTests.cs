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
    }
}