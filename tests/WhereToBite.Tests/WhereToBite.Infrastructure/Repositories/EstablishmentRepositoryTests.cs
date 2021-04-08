using System;
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
        private readonly EstablishmentRepository _establishmentRepository;
        private readonly WhereToBiteContext _whereToBiteContext;

        public EstablishmentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<WhereToBiteContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _whereToBiteContext = new WhereToBiteContext(options);


            _establishmentRepository = new EstablishmentRepository(_whereToBiteContext);
        }

        [Fact]
        public void ShouldCreateInstance()
        {
            Assert.NotNull(_establishmentRepository);
        }

        [Fact(Skip = "pipeline error")]
        public async Task ShouldPersistEstablishment()
        {
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

            await _establishmentRepository.AddIfNotExistsAsync(expectedEstablishment, CancellationToken.None);

            await _establishmentRepository.UnitOfWork.SaveEntitiesAsync();

            var actualEstablishment = _whereToBiteContext.Establishments.FirstOrDefault();

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
            var expectedEstablishment = new Establishment(1,
                "test",
                "Restaurant",
                string.Empty,
                "Pass",
                Point.Empty);

            await _whereToBiteContext.Establishments.AddAsync(expectedEstablishment);
            await _whereToBiteContext.SaveChangesAsync();

            var actual = await _establishmentRepository.GetAsync(expectedEstablishment.Id, CancellationToken.None);
            
            Assert.Equal(expectedEstablishment.Address, actual.Address);
            Assert.Equal(expectedEstablishment.Name, actual.Name);
            Assert.Equal(expectedEstablishment.Type, actual.Type);
            Assert.Equal(expectedEstablishment.EstablishmentStatus, actual.EstablishmentStatus);
        }
        
        [Fact]
        public async Task ShouldGetEstablishmentsWithinRadius()
        {
            var options = new DbContextOptionsBuilder<WhereToBiteContext>()
                .UseInMemoryDatabase("TestDb1")
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

            var actual = await establishmentRepository.GetAllWithinRadiusAsync(1000,new Point(-79.46377746577373, 43.655427942971166), CancellationToken.None);

            var actualEstablishment = Assert.Single(actual);
            
            Assert.NotNull(actualEstablishment);
            Assert.Equal(expectedEstablishment.Address, actualEstablishment.Address);
            Assert.Equal(expectedEstablishment.Name, actualEstablishment.Name);
            Assert.Equal(expectedEstablishment.Type, actualEstablishment.Type);
            Assert.Equal(expectedEstablishment.EstablishmentStatus, actualEstablishment.EstablishmentStatus);
        }
        
        [Fact]
        public async Task ShouldThrowIfCenterIsNull()
        {
            var expectedEstablishment = new Establishment(1,
                "test",
                "Restaurant",
                string.Empty,
                "Pass",
                new Point(-79.45886, 43.65493 ));

            await _whereToBiteContext.Establishments.AddAsync(expectedEstablishment);
            
            await _whereToBiteContext.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentNullException>(() => _establishmentRepository.GetAllWithinRadiusAsync(1000, null!, CancellationToken.None));
        }
        
        [Fact]
        public async Task ShouldThrowIfRadiusIsLessThanZero()
        {
            var expectedEstablishment = new Establishment(1,
                "test",
                "Restaurant",
                string.Empty,
                "Pass",
                new Point(-79.45886, 43.65493 ));

            await _whereToBiteContext.Establishments.AddAsync(expectedEstablishment);
            
            await _whereToBiteContext.SaveChangesAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => _establishmentRepository.GetAllWithinRadiusAsync(-1, Point.Empty, CancellationToken.None));
        }
    }
}