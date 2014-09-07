using System;
using System.Linq;
using System.Linq.Expressions;
using Pagan.Metadata;

namespace Pagan.Linq
{
    // Parts of the Query<T> component that allow for LINQ query compatibility
    public partial class Query<TSource>
    {
        public Query<TSource> Where(Expression<Func<TSource, bool>> condition)
        {
            return this;
        }

        public Query<TSource> Select<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            return this;

        }

        public Query<TResult> Join<TOther, TKey, TResult>(
            Query<TOther> query,
            Expression<Func<TSource, TKey>> outerKeySelector,
            Expression<Func<TOther, TKey>> innerKeySelector,
            Expression<Func<TSource, TOther, TResult>> resultSelector)
        {
            var leftName = _resolver.GetMembers(outerKeySelector);
            var rightName = _resolver.GetMembers(innerKeySelector);

            var leftTable = _factory.GetTable<TSource>();
            var rightTable = _factory.GetTable<TOther>();

            var leftFields = leftTable.GetAllFields(leftName).ToArray();
            var rightField = rightTable.GetAllFields(rightName).ToArray();

            // todo: Build a join from these inputs

            return new Query<TResult>();
        }

        public Query<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            return this;
        }

        public Query<TSource> OrderByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            return this;
        }

        public Query<TSource> ThenBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            return this;
        }

        public Query<TSource> ThenByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            return this;
        }
    }
}
