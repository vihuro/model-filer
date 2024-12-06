using Dapper;
using Microsoft.Extensions.Configuration;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;
using ModelFilter.Domain.Utils.Filters;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace ModelFilter.Persistence.Repository
{
    public abstract class BaseRepositoryDapper<T> : IBaseRepository<T> where T : BaseModel
    {
        protected readonly NpgsqlConnection _connection;
        protected BaseRepositoryDapper(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("postgres");
            _connection = new NpgsqlConnection(connectionString);
        }
        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnDefault<T>> GetAsync(FilterBase? filters, CancellationToken cancellationToken, int maxPerPage = 100)
        {

            var parameters = CreateParameters(filters.Filters);

            var query = CreateQuery(filters, maxPerPage, parameters, false);
            var queryTotalItensInTable = CreateQuery(filters, maxPerPage, parameters);

            var result = await _connection.QueryAsync<T>(query, parameters);
            var totalItems = await _connection.ExecuteScalarAsync<int>(queryTotalItensInTable, parameters);

            var resultDefault = new ReturnDefault<T>()
            {
                CurrentPage = filters.CurrentPage,
                DataResult = result.ToList(),
                TotalItems = (int)Math.Ceiling((decimal)totalItems / maxPerPage)
            };

            return resultDefault;
        }

        private DynamicParameters CreateParameters(List<Filter> filters)
        {
            var parameters = new DynamicParameters();

            var field = string.Empty;
            var value = string.Empty;

            for (int i = 0; i < filters.Count; i++)
            {
                field = "@" + filters[i].Field + i.ToString();
                value = filters[i].Value == null ? null : filters[i].Value?.ToString();

                if (string.Equals(filters[i].Operation, "contains", StringComparison.OrdinalIgnoreCase))
                    value = $"%{value}%";

                parameters.Add(field, value);
            }

            return parameters;
        }

        private string CreateQuery(FilterBase? filters, int maxPerPage, DynamicParameters parameters, bool withPagination = true)
        {
            var typeTable = typeof(T);
            var parameter = Expression.Parameter(typeTable, typeTable.Name.First().ToString());

            var tableName = GetTableName();
            var columns = GetColumns(true);

            var consultation = withPagination ? $"SELECT COUNT(*) FROM \"{tableName}\"" : $"SELECT {columns} FROM \"{tableName}\"";
            var whereClause = CreateWhereClause(filters, parameter, parameters);


            var pagination = withPagination ? "" : CreatePagination(filters, maxPerPage);

            var query = $"{consultation} as \"{parameter}\" {whereClause} {pagination}";

            return query;
        }
        private string CreateWhereClause(FilterBase? filters, Expression parameter, DynamicParameters parameters)
        {
            var filterConditions = filters.Filters
                                           .Select((x, index) =>
                                                    InterpreterFilter(x.Operation, x.Field, x.Value?.ToString(), parameter, index));

            var whereClause = filters.Filters.Count > 0 ? " WHERE " + string.Join(" AND ", filterConditions) : "";


            return whereClause;
        }
        private string CreatePagination(FilterBase? filters, int maxPerPage)
        {
            return $" LIMIT {maxPerPage}";
        }
        private string InterpreterFilter(string operation, string? field,
                                         string valueSearch, Expression parameter,
                                         int index)
        {

            var property = Expression.Property(parameter, field);
            var propertyInfo = (PropertyInfo)property.Member;
            var value = Convert.ChangeType(valueSearch, propertyInfo.PropertyType);

            var options = new Dictionary<string, string>
            {
                { "equals", "{0} = @{1}" },
                { "contains", "{0} LIKE @{1}" },
                { "greaterThanOrEqual","{0} >= @{1}" },
                { "greaterThan","{0} > @{1}" },
                { "lessThanOrEqual","{0} <= @{1}" },
                { "lessThan","{0} < @{1}" }
            };

            var convert = Convert.ChangeType(valueSearch, property.Type);

            options.TryGetValue(operation, out var opertionConverted);

            var columnName = $"\"{propertyInfo.Name}\"";
            var parameterName = propertyInfo.Name + index.ToString();

            //var parameters = string.Format(opertionConverted, $"\"{parameter}\"" + $".\"{propertyInfo.Name}\"", $"{value}");

            var parameters = string.Format(opertionConverted, columnName, parameterName);

            return parameters;
        }

        public void Insert(T entity)
        {
            var table = GetTableName();
            var columns = GetColumns(true);
            var properties = GetPropertyNames(true);

            _connection.Execute($"INSERT INTO {table} ({columns}) VALUES (@properties)", new
            {
                properties
            });
        }

        public void Upate(T entity)
        {
            throw new NotImplementedException();
        }
        private string GetTableName()
        {
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();

            if (tableAttr != null)
            {
                return tableAttr.Name;
            }

            return type.Name + "s";
        }
        private string GetColumns(bool excludeKey = false)
        {
            var type = typeof(T);
            var columnsName = string.Join(", ", type.GetProperties().Select(x =>
            {
                var verifyColumnAttribute = x.GetCustomAttribute<ColumnAttribute>();
                var columnName = verifyColumnAttribute != null ? verifyColumnAttribute.Name : x.Name;

                return $"\"{columnName}\"";
            }));

            return columnsName;
        }
        private string GetPropertyNames(bool exludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                                      .Where(x => !exludeKey ||
                                             x.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join(", ", properties.Select(x =>
            {
                return $"@{x.Name}";
            }));

            return values;

        }
    }
}
