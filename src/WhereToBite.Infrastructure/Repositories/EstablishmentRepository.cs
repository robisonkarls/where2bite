using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;
using WhereToBite.Domain.SeedOfWork;

namespace WhereToBite.Infrastructure.Repositories
{
    public sealed class EstablishmentRepository : IEstablishmentRepository
    {
        private readonly WhereToBiteContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public EstablishmentRepository(WhereToBiteContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<Establishment> AddIfNotExistsAsync(Establishment establishment, CancellationToken cancellationToken)
        {
            
            
            var storedEstablishment = await _context.Establishments
                .SingleOrDefaultAsync(x => x.DineSafeId == establishment.DineSafeId, cancellationToken);

            if (storedEstablishment != null)
            {
                return storedEstablishment;
            }
            
            var newEntry  = await _context.Establishments.AddAsync(establishment, cancellationToken);
            
            return newEntry.Entity;
        }

        private static Expression<Func<Establishment, bool>> FilterByDineSafeId(int dineSafeId)
        {
            return x => x.Id == dineSafeId;
        }

        public async Task<Establishment> GetAsync(int establishmentId, CancellationToken cancellationToken)
        {
            return await _context.Establishments
                .FirstOrDefaultAsync(e => e.Id == establishmentId, cancellationToken);
        }
    }
}