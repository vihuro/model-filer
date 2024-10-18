using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ModelFilter.Domain.Utils.Filters.Expressions
{
    internal class GreaterThanInterpreter<T> : FilterTypeInterpreter<T>
    {
        public GreaterThanInterpreter(Filter filter) : base(filter)
        { }

        internal override Expression CreateExpression(MemberExpression property, ConstantExpression constant)
        {
            var expression = Expression.GreaterThan(property, constant);
            return expression;
        }
    }
}
