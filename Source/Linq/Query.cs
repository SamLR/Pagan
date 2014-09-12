using System;
using System.Linq.Expressions;
using Pagan.Conditions;
using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Linq
{
    public class Query<T>
    {
        private readonly T _current;
        private readonly IQueryBuilder _builder;

        internal Query(T current, IQueryBuilder builder)
        {
            _current = current;
            _builder = builder;
        }

        public Query<T> Where(Expression<Func<T, bool>> filter)
        {
            _builder.AddFilter(GetConditionFromFilter(filter, _current));
            return this;
        }

        public Query<TResult> SelectMany<T2, TResult>(Func<T, Relationship<T2>> getRelationship,
            Func<T, T2, TResult> resultMaker)
        {
            var relationship = getRelationship(_current);
            var related = _builder.Join(relationship);
            return new Query<TResult>(resultMaker(_current, related), _builder);
        }

        public Query<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            var selection = selector(_current);
            _builder.AddSelect(selection);
            return new Query<TResult>(selection, _builder);
        }

        public Query<T> OrderBy(Func<T, Field> sort) 
        {
            _builder.AddSortAsc(sort(_current));
            return this;
        }

        public Query<T> OrderByDescending(Func<T, Field> sort)
        {
            _builder.AddSortDesc(sort(_current));
            return this;
        }

        public Query<T> ThenBy(Func<T, Field> sort)
        {
            _builder.AddSortAsc(sort(_current));
            return this;
        }

        public Query<T> ThenByDescending(Func<T, Field> sort)
        {
            _builder.AddSortDesc(sort(_current));
            return this;
        }
        

        private static Condition GetConditionFromFilter(LambdaExpression ex, T instance)
        {
            var p1 = ex.Parameters[0];
            var ux = (UnaryExpression)ex.Body;

            return Expression.Lambda<Func<T, Condition>>(ux.Operand, p1).Compile()(instance);
        }
    }
}
   

