using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        [Fact]
        public void ShouldPersistEstablishment()
        {
            var expectedEstablishment = new Establishment(1,
                "test",
                "Restaurant",
                string.Empty,
                string.Empty,
                string.Empty,
                EstablishmentStatus.Pass);

            var infraction = new Infraction(SeverityTypes.Minor, ActionTypes.Ticket, DateTime.Now, "", 0m);
            var inspection = new Inspection(InspectionStatus.Pass, DateTime.Now);
            inspection.AddNewInfractions(new[] {infraction});
            var inspections = new[] {inspection};


            expectedEstablishment.AddNewInspections(inspections);

            _establishmentRepository.Add(expectedEstablishment);

            _establishmentRepository.UnitOfWork.SaveChangesAsync();

            var actualEstablishment = _whereToBiteContext.Establishments.FirstOrDefault();

            Assert.NotNull(actualEstablishment);
            Assert.Equal(expectedEstablishment.DineSafeId, actualEstablishment.DineSafeId);
            Assert.Equal(expectedEstablishment.Name, actualEstablishment.Name);
            Assert.Equal(expectedEstablishment.Type, actualEstablishment.Type);
            Assert.Equal(expectedEstablishment.Status, actualEstablishment.Status);

            Assert.NotEmpty(actualEstablishment.Inspections);

            var expectedInspection = expectedEstablishment.Inspections.FirstOrDefault();
            var actualInspection = actualEstablishment.Inspections.FirstOrDefault();
            
            Assert.NotNull(expectedInspection);
            Assert.NotNull(actualInspection);
            
            Assert.Equal(expectedInspection.Status, actualInspection.Status);

            var expectedInfraction = expectedInspection.Infractions.FirstOrDefault();
            var actualInfraction = actualInspection.Infractions.FirstOrDefault();
            
            Assert.NotNull(expectedInfraction);
            Assert.NotNull(actualInfraction);
            
            Assert.Equal(expectedInfraction.Action, actualInfraction.Action);
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
                string.Empty,
                string.Empty,
                EstablishmentStatus.Pass);

            await _whereToBiteContext.Establishments.AddAsync(expectedEstablishment);
            await _whereToBiteContext.SaveChangesAsync();

            var actual = await _establishmentRepository.GetAsync(expectedEstablishment.Id, CancellationToken.None);
            
            Assert.Equal(expectedEstablishment.Address, actual.Address);
            Assert.Equal(expectedEstablishment.Name, actual.Name);
            Assert.Equal(expectedEstablishment.Type, actual.Type);
            Assert.Equal(expectedEstablishment.Status, actual.Status);
        }
    }
}