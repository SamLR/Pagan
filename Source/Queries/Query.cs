using System.Collections.Generic;
using System.Linq;
using Pagan.Relationships;

namespace Pagan.Queries
{
    public class Query: IQueryBuilder, IQuery
    {
        public static implicit operator Query(Table table)
        {
            return new Query(table);
        }

        public static Query operator +(Query parent, Query child)
        {
            parent._childQueries.Add(child);
            return parent;
        }

        private readonly List<Query> _childQueries;
        private FilterExpression _filter;
        private SortingColumn[] _sorting;
        private QueryColumn[] _participants;

        public Query(Table table)
        {
            Table = table;
            _childQueries = new List<Query>();
            Select();
            OrderBy();
        }

        #region Implementation of IQuerySource

        public string Name
        {
            get { return Relationship != null ? Relationship.Name : Table.Name; }
        }
        public Table Table { get; private set; }
        public Relationship Relationship { get; internal set; }

        #endregion

        #region Implementation of IQuery

        public IEnumerable<QueryColumn> Participants { get { return _participants; } }
        public IEnumerable<SortingColumn> Sorting { get { return _sorting; } }
        public FilterExpression Filter { get { return _filter; } }
        public IEnumerable<IQuery> JoinedQueries { get { return _childQueries; } }

        #endregion

        #region Implementation of IQueryBuilder

        public Query Select(params QueryColumn[] selected)
        {
            var keyParts = Table.KeyColumns.Select(x => new QueryColumn(x, true));

            _participants = (selected.Length == 0
                ? keyParts
                : keyParts.Union(selected))
                .ToArray();

            return this;
        }

        public Query OrderBy(params SortingColumn[] sorting)
        {
            _sorting = sorting.Length == 0 
                ? Table.KeyColumns.Select(x => x.Asc()).ToArray() 
                : sorting;

            return this;
        }

        public Query Where(FilterExpression filter)
        {
            _filter = ReferenceEquals(null, _filter)
                ? filter
                : new FilterExpression(_filter, filter, Operators.And);

            return this;
        }

        #endregion

        private int CalculateColumnPositions(int offset = 0)
        {
            _participants.ForEach((p, i) => p.Position = offset + i);

            return _childQueries.Aggregate(offset + _participants.Length, (a, q) => q.CalculateColumnPositions(a));
        }

        internal QueryResult CreateQueryResult()
        {
            var isChild = Relationship != null;

            if (!isChild) CalculateColumnPositions();

            var multi = isChild
                        && Relationship.Role == Role.Dependent
                        && Relationship.Principal.ManyDependents;

            return multi
                ? new QueryMultiResult(Name, _participants, _childQueries)
                : new QueryResult(Name, _participants, _childQueries);
        }

    }
}
