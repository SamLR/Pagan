using System;
using System.Linq;
using System.Text;
using Pagan.Commands;

namespace Pagan.Adapters
{
    class SqlCommandTranslation: SqlDbTranslation
    {
        private readonly ICommand _cmd;

        public SqlCommandTranslation(ICommand cmd)
        {
            _cmd = cmd;
        }

        public string From { get; private set; }
        public string Where { get; private set; }
        public string Update { get; private set; }
        public string Insert { get; private set; }
        public string Select { get; private set; }

        protected override string GetCommandText()
        {
            switch (_cmd.CommandType)
            {
                case CommandType.Insert:
                    return CreateInsertSql();

                case CommandType.Update:
                    return CreateUpdateSql();

                default:
                    return CreateDeleteSql();
            }
        }

        private string CreateInsertSql()
        {
            From = TranslateFromClause();
            Insert = TranslateInsertValues();
            Select = TranslateInsertNames();

            var sql = new StringBuilder();
            sql.Append("INSERT " + From);
            sql.AppendLine(" (" + Select + ")");
            sql.AppendLine("SELECT " + Insert);
            return sql.ToString();

        }
        private string CreateUpdateSql()
        {
            From = TranslateFromClause();
            Where = TranslateWhereClause();
            Update = TranslateUpdateClause();

            var sql = new StringBuilder();
            sql.AppendLine("UPDATE " + From);
            sql.AppendLine(Update);
            sql.AppendLine("WHERE " + Where);
            return sql.ToString();
        }
        private string CreateDeleteSql()
        {
            From = TranslateFromClause();
            Where = TranslateWhereClause();

            var sql = new StringBuilder();
            sql.AppendLine("DELETE FROM " + From);
            sql.AppendLine("WHERE " + Where);
            return sql.ToString();
        }

        private string TranslateFromClause()
        {
            return TranslateTable(_cmd.Table);
        }
        private string TranslateWhereClause()
        {
            return _cmd.Filter != null
                ? TranslateFilterExpression(_cmd.Filter)
                : null;
        }
        private string TranslateInsertNames()
        {
            return String.Join(", ", _cmd.Columns.Select(x => TranslateColumn(x.Column)));
        }
        private string TranslateInsertValues()
        {
            return String.Join(", ", _cmd.Columns.Select(x => TranslateParameter(x.Value)));
        }
        private string TranslateUpdateClause()
        {
            return String.Join(", ", _cmd.Columns.Select(TranslateUpdateColumn));
        }
        private string TranslateUpdateColumn(CommandColumn commandColumn)
        {
            return String.Format("SET {0} = {1}",
                TranslateColumn(commandColumn.Column),
                TranslateParameter(commandColumn.Value)
                );
        }

    }
}