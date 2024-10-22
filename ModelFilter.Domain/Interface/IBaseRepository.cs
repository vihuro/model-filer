using ModelFilter.Domain.Models;
using ModelFilter.Domain.Utils.Filters;

namespace ModelFilter.Domain.Interface
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        Task<ReturnDefault<T>> GetAsync(FilterBaseDto? filters);
        void Insert(T entity);
        void Upate(T entity);
        void Delete(T entity);
    }
}
