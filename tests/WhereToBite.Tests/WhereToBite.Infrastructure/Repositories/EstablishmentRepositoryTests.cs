﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;
using WhereToBite.Infrastructure;
using WhereToBite.Infrastructure.Repositories;
using Xunit;

namespace WhereToBite.Tests.WhereToBite.Infrastructure.Repositories
{
    public class EstablishmentRepositoryTests
    {
        [Fact]
        public void ShouldCreateInstance()
        {
            var options = new DbContextOptionsBuilder<WhereToBiteContext>()
                .UseInMemoryDatabase("test1")
                .Options;

            var whereToBiteContext = new WhereToBiteContext(options);

            var establishmentRepository = new EstablishmentRepository(whereToBiteContext);
            
            Assert.NotNull(establishmentRepository);
        }

        [Fact]
        public async Task ShouldPersistEstablishment()
        {
            var options = new DbContextOptionsBuilder<WhereToBiteContext>()
                .UseInMemoryDatabase("test2")
                .Options;

            var whereToBiteContext = new WhereToBiteContext(options);

            var establishmentRepository = new EstablishmentRepository(whereToBiteContext);
            
            var expectedEstablishment = new Establishment(1,
                "test",
                "Restaurant",
                string.Empty,
                "Pass", 
                Point.Empty);

            var infraction = new Infraction("M - Minor", "Ticket", DateTime.Now, "", 0m);
            var inspection = new Inspection("Pass", DateTime.Now);
            inspection.AddNewInfractions(new[] {infraction});
            var inspections = new[] {inspection};


            expectedEstablishment.AddNewInspections(inspections);

            await establishmentRepository.AddIfNotExistsAsync(expectedEstablishment, CancellationToken.None);

            await establishmentRepository.UnitOfWork.SaveEntitiesAsync();

            var actualEstablishment = whereToBiteContext.Establishments.FirstOrDefault();

            Assert.NotNull(actualEstablishment);
            Assert.Equal(expectedEstablishment.DineSafeId, actualEstablishment.DineSafeId);
            Assert.Equal(expectedEstablishment.Name, actualEstablishment.Name);
            Assert.Equal(expectedEstablishment.Type, actualEstablishment.Type);
            Assert.Equal(expectedEstablishment.EstablishmentStatus, actualEstablishment.EstablishmentStatus);

            Assert.NotEmpty(actualEstablishment.Inspections);

            var expectedInspection = expectedEstablishment.Inspections.FirstOrDefault();
            var actualInspection = actualEstablishment.Inspections.FirstOrDefault();
            
            Assert.NotNull(expectedInspection);
            Assert.NotNull(actualInspection);
            
            Assert.Equal(expectedInspection.InspectionStatus, actualInspection.InspectionStatus);

            var expectedInfraction = expectedInspection.Infractions.FirstOrDefault();
            var actualInfraction = actualInspection.Infractions.FirstOrDefault();
            
            Assert.NotNull(expectedInfraction);
            Assert.NotNull(actualInfraction);
            
            Assert.Equal(expectedInfraction.InfractionAction, actualInfraction.InfractionAction);
            Assert.Equal(expectedInfraction.Severity, actualInfraction.Severity);
            Assert.Equal(expectedInfraction.AmountFined, actualInfraction.AmountFined);
        }

        [Fact]
        public async Task ShouldGetSavedEstablishment()
        {
            var options = new DbContextOptionsBuilder<WhereToBiteContext>()
                .UseInMemoryDatabase("test3")
                .Options;

            var whereToBiteContext = new WhereToBiteContext(options);

            var establishmentRepository = new EstablishmentRepository(whereToBiteContext);

            var expectedEstablishment = new Establishment(1,
                "test",
                "Restaurant",
                string.Empty,
                "Pass",
                Point.Empty);

            await whereToBiteContext.Establishments.AddAsync(expectedEstablishment);
            await whereToBiteContext.SaveChangesAsync();

            var actual = await establishmentRepository.GetAsync(expectedEstablishment.Id, CancellationToken.None);
            
            Assert.Equal(expectedEstablishment.Address, actual.Address);
            Assert.Equal(expectedEstablishment.Name, actual.Name);
            Assert.Equal(expectedEstablishment.Type, actual.Type);
            Assert.Equal(expectedEstablishment.EstablishmentStatus, actual.EstablishmentStatus);
        }

        [Fact]
        public async Task ShouldGetEstablishmentsWithinRadius()
        {
            var options = new DbContextOptionsBuilder<WhereToBiteContext>()
                .UseInMemoryDatabase("test4")
                .Options;

            var whereToBiteContext = new WhereToBiteContext(options);

            var establishmentRepository = new EstablishmentRepository(whereToBiteContext);
            
            var expectedEstablishment = new Establishment(
                1,
                "test",
                "Restaurant",
                string.Empty,
                "Pass",
                new Point(-79.45886, 43.65493 ));
            
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
            
            expectedEstablishment.AddNewInspections(new []{expectedInspection});

            await whereToBiteContext.Establishments.AddAsync(expectedEstablishment);
            await whereToBiteContext.SaveChangesAsync();

            var actual = await establishmentRepository.GetAllWithinRadiusAsync(
                1000, 
                new Point(-79.46377746577373, 43.655427942971166), 
                CancellationToken.None);

            var actualEstablishment = Assert.Single(actual);
            
            Assert.NotNull(actualEstablishment);
            Assert.Equal(expectedEstablishment.Address, actualEstablishment.Address);
            Assert.Equal(expectedEstablishment.Name, actualEstablishment.Name);
            Assert.Equal(expectedEstablishment.Type, actualEstablishment.Type);
            Assert.Equal(expectedEstablishment.EstablishmentStatus, actualEstablishment.EstablishmentStatus);
            Assert.Equal(
                expectedInfraction.AmountFined + expectedInfractionTwo.AmountFined, 
                actualEstablishment.Inspections.Select(x => x.Infractions.Sum(infraction => infraction.AmountFined)).Sum());
        }
        
        [Fact]
        public async Task ShouldThrowIfCenterIsNull()
        {
            
            var options = new DbContextOptionsBuilder<WhereToBiteContext>()
                .UseInMemoryDatabase("test5")
                .Options;

            var whereToBiteContext = new WhereToBiteContext(options);

            var establishmentRepository = new EstablishmentRepository(whereToBiteContext);
            
            var expectedEstablishment = new Establishment(1,
                "test",
                "Restaurant",
                string.Empty,
                "Pass",
                new Point(-79.45886, 43.65493 ));

            await whereToBiteContext.Establishments.AddAsync(expectedEstablishment);
            
            await whereToBiteContext.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentNullException>(() => establishmentRepository.GetAllWithinRadiusAsync(1000, null!, CancellationToken.None));
        }
        
        [Fact]
        public async Task ShouldThrowIfRadiusIsLessThanZero()
        {
            var options = new DbContextOptionsBuilder<WhereToBiteContext>()
                .UseInMemoryDatabase("test6")
                .Options;

            var whereToBiteContext = new WhereToBiteContext(options);

            var establishmentRepository = new EstablishmentRepository(whereToBiteContext);
            
            var expectedEstablishment = new Establishment(
                1,
                "test",
                "Restaurant",
                string.Empty,
                "Pass",
                new Point(-79.45886, 43.65493 ));

            await whereToBiteContext.Establishments.AddAsync(expectedEstablishment);
            
            await whereToBiteContext.SaveChangesAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => establishmentRepository.GetAllWithinRadiusAsync(-1, Point.Empty, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldReturnEstablishmentInspections()
        {
            var options = new DbContextOptionsBuilder<WhereToBiteContext>()
                .UseInMemoryDatabase("test5")
                .Options;

            var whereToBiteContext = new WhereToBiteContext(options);

            var establishmentRepository = new EstablishmentRepository(whereToBiteContext);
            
            var expectedEstablishment = new Establishment(
                1,
                "test",
                "Restaurant",
                string.Empty,
                "Pass",
                new Point(-79.45886, 43.65493 ));
            
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
            
            expectedEstablishment.AddNewInspections(new []{expectedInspection});

            await whereToBiteContext.Establishments.AddAsync(expectedEstablishment);
            await whereToBiteContext.SaveChangesAsync();
            
            // act
            var actual = await establishmentRepository.GetInspectionsAsync(1, CancellationToken.None);
            
            Assert.NotNull(actual);
            var actualInspection = Assert.Single(actual);
            Assert.NotNull(actualInspection);
            Assert.Equal(expectedInspection.Date, actualInspection.Date);
            Assert.Equal(expectedInspection.Id, actualInspection.Id);
            Assert.Equal(expectedInspection.InspectionStatus, actualInspection.InspectionStatus);
            Assert.Equal(expectedInspection.Infractions.Count, actualInspection.Infractions.Count);
        }
    }
}