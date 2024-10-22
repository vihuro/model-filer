using Microsoft.EntityFrameworkCore;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;
using ModelFilter.Domain.Utils.Filters;
using ModelFilter.Persistence.Context;
using System.Linq.Expressions;

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

            query = ApplyOrder((IOrderedQueryable<T>)query, filters.MultiSort);

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
        private IOrderedQueryable<T> ApplyOrder<T>(IOrderedQueryable<T> query, List<MultiSort> multiSorts)
        {
            var functionsOnQueryable = typeof(Queryable).GetMethods();
            var thenBy = functionsOnQueryable.Where(x => x.Name == "ThenBy" && x.GetParameters().Length == 2).FirstOrDefault();
            var thenByDescending = functionsOnQueryable.Where(x => x.Name == "ThenByDescending" && x.GetParameters().Length == 2).FirstOrDefault();
            foreach (var item in multiSorts)
            {
                var param = Expression.Parameter(typeof(T), "x"); // Parâmetro da expressão
                var property = Expression.Property(param, item.Field); // Propriedade a ser ordenada
                var keySelector = Expression.Lambda(property, param); // Criação da expressão lambda

                var genericThenBy = thenBy.MakeGenericMethod(typeof(T), property.Type);
                var genericThenByDescending = thenByDescending.MakeGenericMethod(typeof(T), property.Type);

                // Verificação do tipo de ordenação
                if (item.Value.Equals(EOrderByType.Asc.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    query = (IOrderedQueryable<T>)genericThenBy.Invoke(null, new object[] { query, keySelector });
                }
                else if (item.Value.Equals(EOrderByType.Desc.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    query = (IOrderedQueryable<T>)genericThenByDescending.Invoke(null, new object[] { query, keySelector });
                }
            }
            return query;
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
