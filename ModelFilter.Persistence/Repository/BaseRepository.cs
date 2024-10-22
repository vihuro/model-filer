using Microsoft.EntityFrameworkCore;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;
using ModelFilter.Domain.Utils.Filters;
using ModelFilter.Persistence.Context;

namespace ModelFilter.Persistence.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        protected readonly AppDbContext appDbContext;
        private readonly IFilterDynamic _filterDynamic;

        protected BaseRepository(AppDbContext appDbContext, IFilterDynamic filterDynamic)
        {
            this.appDbContext = appDbContext;
            _filterDynamic = filterDynamic;
        }

        public Task Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnDefault<T>> GetAsync(FilterBase? filters, int maxPerPage = 100)
        {
            var expression = _filterDynamic.FromFilterList<T>(filters);

            var query = appDbContext.Set<T>().Where(expression)
                                             .Skip((filters.CurrentPage - 1) * filters.MaxPerPage)
                                             .Take(filters.MaxPerPage <= maxPerPage ? filters.MaxPerPage : maxPerPage)
                                             .OrderBy(x => x).AsQueryable();

            if (filters.MultiSort?.Count > 0)
            {
                foreach (var item in filters.MultiSort)
                {
                    query = ((IOrderedQueryable<T>)query).ThenBy(x => EF.Property<object>(x, item.Field));
                }
            }
            var data = await query.ToListAsync();

            var totalItems = await appDbContext.Set<T>().Where(expression)
                                  .CountAsync();

            var result = new ReturnDefault<T>()
            {
                CurrentPage = filters.CurrentPage,
                TotalItems = totalItems,
                MaxPerPage = filters.MaxPerPage,
                TotalPages = totalItems < filters.MaxPerPage && data.Count > 0 ? 1 : totalItems / filters.MaxPerPage,
                DataResult = data
            };

            return result;

        }

        public void Insert(T entity)
        {
            entity.DateCreated = DateTime.UtcNow;
            appDbContext.Add(entity);
        }

        public void Upate(T entity)
        {
            entity.DateUpdated = DateTime.UtcNow;
            appDbContext.Update(entity);
        }

        void IBaseRepository<T>.Delete(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
