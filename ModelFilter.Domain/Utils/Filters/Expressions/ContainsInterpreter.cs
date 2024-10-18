using System.Linq.Expressions;

namespace ModelFilter.Domain.Utils.Filters.Expressions
{
    public class ContainsInterpreter<T> : FilterTypeInterpreter<T>
    {
        public ContainsInterpreter(Filter filtroItem) : base(filtroItem)
        {
        }

        internal override Expression CreateExpression(MemberExpression property, ConstantExpression constant)
        {
            var method = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
            var expression = Expression.Call(property, method, constant);

            return expression;
        }
    }
}
