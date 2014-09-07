using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Pagan.Linq
{
    class ExpressionResolver
    {
        public IEnumerable<string> GetMembers(LambdaExpression ex)
        {
            if (ex == null) throw new ArgumentNullException("ex");

            var mx = ex.Body as MemberExpression;
            if (mx != null) return new[] {mx.Member.Name};

            var cx = ex.Body as NewExpression;
            if (cx == null) throw new ArgumentException("Expression body must be either MemberExpression or NewExpression");

            return cx.Arguments.Cast<MemberExpression>().Select(m => m.Member.Name);
        }
    }
}