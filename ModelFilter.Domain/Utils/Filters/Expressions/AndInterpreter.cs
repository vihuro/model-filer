using ModelFilter.Domain.Interface;
using System.Linq.Expressions;

namespace ModelFilter.Domain.Utils.Filters.Expressions
{
    public class AndInterpreter<T> : IFilterTypeInterpreter<T>
    {
        private readonly IFilterTypeInterpreter<T> _leftInterpreter;
        private readonly IFilterTypeInterpreter<T> _rightInterpreter;

        public AndInterpreter(IFilterTypeInterpreter<T> leftInterpreter, IFilterTypeInterpreter<T> rightInterpreter)
        {
            _leftInterpreter = leftInterpreter;
            _rightInterpreter = rightInterpreter;
        }

        public Expression<Func<T, bool>> Interpret()
        {
            var leftExpression = _leftInterpreter.Interpret();
            var rightExpression = Expression.Invoke(_rightInterpreter.Interpret(), leftExpression.Parameters.FirstOrDefault());

            var andAlsoExpression = Expression.AndAlso(leftExpression.Body, rightExpression);

            return Expression.Lambda<Func<T, bool>>(andAlsoExpression, leftExpression.Parameters.FirstOrDefault());
        }
    }
}
