using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Infrastructure.Repositories
{
    public class EstablishmentRepository : IEstablishmentRepository
    {
        private readonly WhereToBiteContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public EstablishmentRepository(WhereToBiteContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public Establishment Add(Establishment establishment)
        {
            return _context.Establishments.Add(establishment).Entity;
        }

        public async Task<Establishment> GetAsync(int establishmentId, CancellationToken cancellationToken)
        {
            return await _context.Establishments
                .FirstOrDefaultAsync(e => e.Id == establishmentId, cancellationToken);
        }
    }
}