using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using Pagan.Queries;

namespace Pagan.Adapters
{
    abstract class SqlDbTranslation : IDbTranslation
    {
        protected const string TableFormat = "[{0}].[{1}]";
        protected const string ColumnFormat = "{0}.[{1}]";
        protected const string BinaryFormat = "{0} {1} {2}";

        protected SqlDbTranslation()
        {
            Parameters = new Dictionary<string, object>();
        }

        public virtual void BuildCommand(IDbCommand cmd)
        {
            cmd.CommandText = GetCommandText();
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;

#if DEBUG
            Debug.WriteLine(cmd.CommandText);
#endif

            Parameters.ForEach(x =>
            {
#if DEBUG
                Debug.WriteLine(x.Key + ": " + x.Value);
#endif
                var p = cmd.CreateParameter();
                p.ParameterName = x.Key;
                p.Value = x.Value;
                p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p);
            });
        }

        protected abstract string GetCommandText();
        protected IDictionary<string, object> Parameters;

        protected static string TranslateTable(Table table)
        {
            return string.Format(TableFormat, table.Schema.DbName, table.DbName);
        }
        protected static string TranslateColumn(Column column)
        {
            return String.Format(ColumnFormat, TranslateTable(column.Table), column.DbName);
        }
        protected string TranslateFilterExpression(FilterExpression filter)
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

                case Operators.StartsWith:
                case Operators.EndsWith:
                case Operators.Contains:
                    op = "LIKE";
                    break;

                case Operators.NotStartsWith:
                case Operators.NotEndsWith:
                case Operators.NotContains:
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
        protected string TranslateLeftFilterOperand(FilterExpression filter)
        {
            if (filter.Left != null) return TranslateFilterExpression(filter.Left);
            if (!ReferenceEquals(filter.LeftColumn, null)) return TranslateColumn(filter.LeftColumn);
            throw new Exception("Unexpected left operand in FilterExpression");
        }
        protected string TranslateRightFilterOperand(FilterExpression filter)
        {
            if (filter.Right != null) return TranslateFilterExpression(filter.Right);
            if (!ReferenceEquals(filter.RightColumn, null)) return TranslateColumn(filter.RightColumn);
            if (filter.Value == null) return "NULL";
            if (filter.Value is bool) return (bool)filter.Value ? "1" : "0";

            return filter.Value.GetType().IsArray
                ? String.Format("({0})", TranslateValueArray((IEnumerable) filter.Value))
                : TranslateParameter(filter.Value);
        }

        protected string TranslateValueArray(IEnumerable array)
        {
            return String.Join(", ", array.OfType<object>().Select(TranslateParameter));
        }

        protected string TranslateParameter(object value)
        {
            var name = "@p" + Parameters.Count;
            Parameters.Add(name, value);
            return name;
        }

    }
}