using System.Threading;
using System.Threading.Tasks;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public interface IEstablishmentRepository : IRepository<Establishment>
    {
        Task<Establishment> AddIfNotExistsAsync(Establishment establishment, CancellationToken cancellationToken);
        Task<Establishment> GetAsync(int establishmentId, CancellationToken cancellationToken);
    }
}