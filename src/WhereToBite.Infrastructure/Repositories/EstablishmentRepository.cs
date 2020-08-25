using System;
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
            return _context.Add(establishment).Entity;
        }

        public async Task<Establishment> GetAsync(int establishmentId)
        {
            var establishment = await _context.Establishments
                .FirstOrDefaultAsync(e => e.Id == establishmentId);
            return establishment;
        }
    }
}