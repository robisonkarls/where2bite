using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public interface IEstablishmentRepository : IRepository<Establishment>
    {
        Task<Establishment> AddIfNotExistsAsync(Establishment establishment, CancellationToken cancellationToken);
        Task<Establishment> GetAsync(int establishmentId, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<Establishment>> GetAllWithinRadiusAsync(double radiusSizeInMeters, Point center,
            CancellationToken cancellationToken);
        Task<IReadOnlyCollection<Inspection>> GetInspectionsAsync(int establishmentId,
            CancellationToken cancellationToken);
    }
}