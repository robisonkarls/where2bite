using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;
using WhereToBite.Domain.SeedOfWork;
using WhereToBite.Infrastructure.EntityConfiguration;

namespace WhereToBite.Infrastructure
{
    public class WhereToBiteContext : DbContext, IUnitOfWork
    {
        public const string DefaultSchema = "w2b";

        public DbSet<Establishment> Establishments { get; set; }

        private IDbContextTransaction _currentTransaction;

        public WhereToBiteContext(DbContextOptions<WhereToBiteContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EstablishmentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EstablishmentStatusEntityConfiguration());
            modelBuilder.ApplyConfiguration(new InfractionActionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new InfractionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new InspectionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new InspectionStatusEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SeverityEntityConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        public IDbContextTransaction GetCurrentTransaction => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (HasActiveTransaction)
            {
                return null;
            }

            return await Database.BeginTransactionAsync();
        }
        
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}