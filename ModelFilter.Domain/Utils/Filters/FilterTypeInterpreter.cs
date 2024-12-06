using ModelFilter.Domain.Interface;
using System.Linq.Expressions;
using System.Reflection;

namespace ModelFilter.Domain.Utils.Filters
{
    public abstract class FilterTypeInterpreter<T> : IFilterTypeInterpreter<T>
    {
        private readonly Filter _filter;

        public FilterTypeInterpreter(Filter filter)
        {
            _filter = filter;
        }

        public Expression<Func<T, bool>> Interpret()
        {
            var dynamicType = typeof(T);
            var parameter = Expression.Parameter(dynamicType, dynamicType.Name.First().ToString());
            var property = Expression.Property(parameter, _filter.Field);
            var propertyInfo = (PropertyInfo)property.Member;
            var value = Convert.ChangeType(_filter.Value?.ToString(), propertyInfo.PropertyType);

            if(value is DateTime dateTimeValue)
            {
                if(dateTimeValue.Kind == DateTimeKind.Utc)
                {
                    dateTimeValue = DateTime.SpecifyKind(dateTimeValue, DateTimeKind.Utc);
                }
                else
                {
                    dateTimeValue = dateTimeValue.ToUniversalTime();
                }
                value = dateTimeValue;
            }

            var constant = Expression.Constant(value);
            var expression = CreateExpression(property, constant);

            return Expression.Lambda<Func<T, bool>>(expression, parameter);
        }
        internal abstract Expression CreateExpression(MemberExpression property, ConstantExpression constant);

    }
}
