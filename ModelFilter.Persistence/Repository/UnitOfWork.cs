using Microsoft.EntityFrameworkCore.Storage;
using ModelFilter.Domain.Interface;
using ModelFilter.Persistence.Context;

namespace ModelFilter.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Commit(CancellationToken cancellationToken)
        {
            await _appDbContext.SaveChangesAsync(cancellationToken);
            Dispose();
        }
        public async Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken)
        {
            return await _appDbContext.Database.BeginTransactionAsync(cancellationToken);
        }
        public async Task Commit(IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            await _appDbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            Dispose();
        }

        public void Dispose()
        {
            _appDbContext?.Dispose();
            
            GC.SuppressFinalize(this);
        }
    }
}
