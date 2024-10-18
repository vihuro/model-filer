using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ModelFilter.Domain.Utils.Filters.Expressions
{
    public class LessThanOrEqualInterpreter<T> : FilterTypeInterpreter<T>
    {
        public LessThanOrEqualInterpreter(Filter filter) : base(filter)
        { }

        internal override Expression CreateExpression(MemberExpression property, ConstantExpression constant)
        {
            var expression = Expression.LessThanOrEqual(property, constant);

            return expression;
        }
    }
}
