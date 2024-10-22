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

        public async Task<ReturnDefault<T>> GetAsync(FilterBase? filters, CancellationToken cancellationToken, int maxPerPage = 100)
        {

            var expression = _filterDynamic.FromFilterList<T>(filters);

            var maxPerPageUsing = filters.MaxPerPage <= maxPerPage ? filters.MaxPerPage : maxPerPage;

            var query = appDbContext.Set<T>().Where(expression);

            query = ApplyOrder((IOrderedQueryable<T>)query, filters.MultiSort);

            query = query.Skip((filters.CurrentPage - 1) * filters.MaxPerPage)
                         .Take(maxPerPageUsing);

            var data = await query.ToListAsync(cancellationToken);

            var totalItems = await appDbContext.Set<T>().Where(expression)
                                                        .CountAsync(cancellationToken);

            var totalPages = Math.Ceiling((decimal)totalItems / maxPerPageUsing);

            var result = new ReturnDefault<T>()
            {
                CurrentPage = filters.CurrentPage,
                TotalItems = totalItems,
                MaxPerPage = maxPerPageUsing,
                TotalPages = (int)totalPages,
                DataResult = data
            };

            return result;

        }
        private IQueryable<T> ApplyOrder(IQueryable<T> query, List<MultiSort> multiSorts)
        {
            if (multiSorts?.Count < 1) return query;

            var functionsOnQueryable = typeof(Queryable).GetMethods();
            var OrderBy = functionsOnQueryable.First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
            var OrderByDescending = functionsOnQueryable.First(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2);
            var thenBy = functionsOnQueryable.First(x => x.Name == "ThenBy" && x.GetParameters().Length == 2);
            var thenByDescending = functionsOnQueryable.First(x => x.Name == "ThenByDescending" && x.GetParameters().Length == 2);

            IOrderedQueryable<T> orderedQuery = null;

            foreach (var item in multiSorts)
            {
                var param = Expression.Parameter(typeof(T), "x"); // Parâmetro da expressão
                var property = Expression.Property(param, item.Field); // Propriedade a ser ordenada
                var keySelector = Expression.Lambda(property, param); // Criação da expressão lambda

                var genericThenBy = thenBy.MakeGenericMethod(typeof(T), property.Type);
                var genericThenByDescending = thenByDescending.MakeGenericMethod(typeof(T), property.Type);
                var genericOrderBy = OrderBy.MakeGenericMethod(typeof(T), property.Type);
                var genericOrderByDesceding = OrderByDescending.MakeGenericMethod(typeof(T), property.Type);

                // Verificação do tipo de ordenação
                if (item.Value.Equals(EOrderByType.Asc.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    orderedQuery = (orderedQuery == null)
                        ? (IOrderedQueryable<T>)genericOrderBy.Invoke(null, new object[] { query, keySelector })
                        : (IOrderedQueryable<T>)genericThenBy.Invoke(null, new object[] { orderedQuery, keySelector });
                }
                else if (item.Value.Equals(EOrderByType.Desc.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    orderedQuery = (orderedQuery == null)
                        ? (IOrderedQueryable<T>)genericOrderByDesceding.Invoke(null, new object[] { query, keySelector })
                        : (IOrderedQueryable<T>)genericThenByDescending.Invoke(null, new object[] { orderedQuery, keySelector });
                }
            }

            return orderedQuery ?? query.OrderBy(x => x); // Retorna a query ordenada ou a original se nenhuma ordenação foi aplicada
        }

        public void Insert(T entity)
        {
            entity.DateCreated = DateTime.UtcNow;
            entity.DateUpdated = DateTime.UtcNow;
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
