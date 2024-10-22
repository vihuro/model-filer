namespace ModelFilter.Domain.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit(CancellationToken cancellationToken);
    }
}
