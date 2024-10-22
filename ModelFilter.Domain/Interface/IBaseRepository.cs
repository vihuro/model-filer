using ModelFilter.Domain.Models;
using ModelFilter.Domain.Utils.Filters;

namespace ModelFilter.Domain.Interface
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        Task<ReturnDefault<T>> GetAsync(FilterBase? filters, CancellationToken cancellationToken, int maxPerPage = 100);
        void Insert(T entity);
        void Upate(T entity);
        void Delete(T entity);
    }
}
