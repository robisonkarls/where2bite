using System.Threading;
using System.Threading.Tasks;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Domain.AggregatesModel.EstablishmentAggregate
{
    public interface IEstablishmentRepository : IRepository<Establishment>
    {
        Establishment Add(Establishment establishment);
        Task<Establishment> GetAsync(int establishmentId, CancellationToken cancellationToken);
    }
}