using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pagan.Queries;

namespace Pagan.Adapters
{
    class SqlQueryTranslation: SqlDbTranslation
    {
        private const string SortedColumnFormat = "{0} {1}";
        private const string JoinFormat = "{0} {1} ON {2}";

        public SqlQueryTranslation(IQuery query)
        {
            _query = query;
        }

        private readonly IQuery _query;

        public string Select { get; private set; }
        public string From { get; private set; }
        public string Where { get; private set; }
        public string OrderBy { get; private set; }
        public IEnumerable<string> Joins { get; private set; }

        public override string GetCommandText()
        {
            From = TranslateFromClause();
            Where = TranslateWhereClause();
            Select = TranslateSelectClause();
            OrderBy = TranslateOrderByClause();
            Joins = TranslateJoinClauses();

            var sql = new StringBuilder();
            sql.AppendLine("SELECT " + Select);
            sql.AppendLine("FROM " + From);
            Joins.ForEach(x => sql.AppendLine(x));
            if (!String.IsNullOrEmpty(Where)) sql.AppendLine("WHERE " + Where);
            if (!String.IsNullOrEmpty(OrderBy)) sql.AppendLine("ORDER BY " + OrderBy);

            return sql.ToString();
        }

        private string TranslateFromClause()
        {
            return TranslateTable(_query.Table);
        }
        private string TranslateSelectClause()
        {
            var all = _query.Participants.Union(_query.JoinedQueries.SelectMany(q => q.Participants));
            return String.Join(", ", all.Select(TranslateSelectedColumn));
        }
        private IEnumerable<string> TranslateJoinClauses()
        {
            return _query.JoinedQueries.Select(TranslateJoin);
        }
        private string TranslateWhereClause()
        {
            return _query.Filter != null
                ? TranslateFilterExpression(_query.Filter)
                : null;
        }
        private string TranslateOrderByClause()
        {
            var all = _query.Sorting.Union(_query.JoinedQueries.SelectMany(q => q.Sorting));
            return String.Join(", ", all.Select(TranslateSortedColumn));
        }
        private string TranslateJoin(IQuery query)
        {
            return String.Format(JoinFormat,
                "LEFT JOIN",
                TranslateTable(query.Table),
                TranslateFilterExpression(query.Relationship.JoinExpression));
        }
        
        private static string TranslateSelectedColumn(QueryColumn participant)
        {
            return TranslateColumn(participant.Column);
        }
        private static string TranslateSortedColumn(SortingColumn sortedColumn)
        {
            return String.Format(
                SortedColumnFormat,
                TranslateColumn(sortedColumn.Column),
                sortedColumn.Direction == SortDirection.Ascending ? "ASC" : "DESC");
        }
        

    }
}