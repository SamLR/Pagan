using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Pagan.Commands;
using Pagan.Queries;
using CommandType = System.Data.CommandType;

namespace Pagan.Adapters
{
    public class SqlQueryAdapter : IQueryAdapter
    {
        private const string TableFormat = "[{0}].[{1}]";
        private const string ColumnFormat = "{0}.[{1}]";
        private const string SortedColumnFormat = "{0} {1}";
        private const string JoinFormat = "{0} {1} ON {2}";
        private const string BinaryFormat = "{0} {1} {2}";

        private Query _query;
        private IDictionary<string, object> _queryArgs;

        public void TranslateQuery(Query query, DbCommand dbCommand)
        {
            _query = query;
            _queryArgs = new Dictionary<string, object>();
            TranslateSelectClause();
            TranslateFromClause();
            TranslateJoinClauses();
            TranslateWhereClause();
            TranslateOrderByClause();

            dbCommand.CommandText = CreateSql();
            dbCommand.CommandType = CommandType.Text;
            dbCommand.CommandTimeout = 0;
            CreateDbParameters(dbCommand);
        }

        public void TranslateCommand(Command paganCommand, DbCommand dbCommand)
        {
            throw new NotImplementedException();
        }

        public string Select { get; private set; }
        public string From { get; private set; }
        public string OrderBy { get; private set; }
        public string Where { get; private set; }
        public IEnumerable<string> Joins { get; private set; }
        
        private void CreateDbParameters(DbCommand cmd)
        {
            _queryArgs.ForEach(x =>
            {
                var p = cmd.CreateParameter();
                p.ParameterName = x.Key;
                p.Value = x.Value;
                p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p);
            });
        }

        private string CreateSql()
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT " + Select);
            sql.AppendLine("FROM " + From);
            Joins.ForEach(x => sql.AppendLine(x));
            if (!String.IsNullOrEmpty(Where)) sql.AppendLine("WHERE " + Where);
            if (!String.IsNullOrEmpty(OrderBy)) sql.AppendLine("ORDER BY " + OrderBy);
            return sql.ToString();
        }

        private void TranslateFromClause()
        {
            From = TranslateTable(_query.Table);
        }

        private void TranslateSelectClause()
        {
            var all = _query.Participants.Union(_query.JoinedQueries.SelectMany(q => q.Participants));
            Select = String.Join(", ", all.Select(TranslateSelectedColumn));
        }

        private void TranslateJoinClauses()
        {
            Joins = _query.JoinedQueries.Select(TranslateJoin);
        }

        private void TranslateWhereClause()
        {
            if (_query.Filter != null)
                Where = TranslateFilterExpression(_query.Filter);
        }

        private void TranslateOrderByClause()
        {
            var all = _query.Sorting.Union(_query.JoinedQueries.SelectMany(q => q.Sorting));
            OrderBy = String.Join(", ", all.Select(TranslateSortedColumn));
        }

        private static string TranslateTable(Table table)
        {
            return string.Format(TableFormat, table.Schema.DbName, table.DbName);
        }

        private static string TranslateColumn(Column column)
        {
            return String.Format(ColumnFormat, TranslateTable(column.Table), column.DbName);
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

        private string TranslateJoin(IQuery query)
        {
            return String.Format(JoinFormat,
                "LEFT JOIN",
                TranslateTable(query.Table),
                TranslateFilterExpression(query.Relationship.JoinExpression));
        }

        private string TranslateFilterExpression(FilterExpression filter)
        {
            var left = TranslateLeftFilterOperand(filter);
            var right = TranslateRightFilterOperand(filter);
            string op = null;
            switch (filter.Operator)
            {
                case Operators.And:
                    op = "AND";
                    break;

                case Operators.Or:
                    op = "OR";
                    break;

                case Operators.Equal:
                    op = "=";
                    break;

                case Operators.NotEqual:
                    op = "!=";
                    break;

                case Operators.GreaterOrEqual:
                    op = ">=";
                    break;

                case Operators.LessOrEqual:
                    op = "<=";
                    break;

                case Operators.GreaterThan:
                    op = ">";
                    break;

                case Operators.LessThan:
                    op = "<=";
                    break;

                case Operators.IsNull:
                    op = "IS";
                    break;

                case Operators.IsNotNull:
                    op = "IS NOT";
                    break;

                case Operators.Like:
                    op = "LIKE";
                    break;

                case Operators.Unlike:
                    op = "NOT LIKE";
                    break;

                case Operators.In:
                    op = "IN";
                    break;

                case Operators.NotIn:
                    op = "NOT IN";
                    break;
            }
            return String.Format(BinaryFormat, left, op, right);
        }

        private string TranslateLeftFilterOperand(FilterExpression filter)
        {
            if (filter.Left != null) return TranslateFilterExpression(filter.Left);
            if (!ReferenceEquals(filter.LeftColumn, null)) return TranslateColumn(filter.LeftColumn);
            throw new Exception("Unexpected left operand in FilterExpression");
        }

        private string TranslateRightFilterOperand(FilterExpression filter)
        {
            if (filter.Right != null) return TranslateFilterExpression(filter.Right);
            if (!ReferenceEquals(filter.RightColumn, null)) return TranslateColumn(filter.RightColumn);
            if (filter.Value == null) return "NULL";
            if (filter.Value is bool) return (bool) filter.Value ? "1" : "0";
            return TranslateParameter(filter.Value);
        }

        private string TranslateParameter(object value)
        {
            var name = "@p" + _queryArgs.Count;
            _queryArgs.Add(name, value);
            return name;
        }
    }
}