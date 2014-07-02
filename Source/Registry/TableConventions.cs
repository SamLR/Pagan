using System;
using System.Collections.Generic;
using System.Linq;

namespace Pagan.Registry
{
    public class TableConventions: ITableConventions
    {
        public string GetSchemaDbName(Table table)
        {
            return "dbo";
        }

        public string GetTableDbName(Table table)
        {
            return table.Name;
        }

        public string GetColumnDbName(Column column)
        {
            return column.Name;
        }

        public Column[] GetPrimaryKey(Table table)
        {
            Column column;

            return
                table.TryGetDbColumn(TableKeyMatchList(table), out column)
                    ? new[] {column}
                    : new Column[0];
        }

        public Column[] GetForeignKey(Table principalTable, Table dependentTable)
        {
            var matchList = TableKeyMatchList(principalTable);
            var fkCols = new List<Column>();

            foreach (
                var column in
                    principalTable.KeyColumns.Select(
                        key => dependentTable.Columns.FirstOrDefault(candidate => Match(key, candidate, matchList))))
            {
                if (ReferenceEquals(null, column)) return new Column[0];
                fkCols.Add(column);
            }

            return fkCols.ToArray();
        }

        private static bool Match(Column key, Column candidate, string[] matchList)
        {
            return
                string.Equals(key.DbName, candidate.DbName, StringComparison.InvariantCultureIgnoreCase)
                || (
                    matchList.Any(m => String.Equals(m, key.DbName, StringComparison.InvariantCultureIgnoreCase))
                    &&
                    matchList.Skip(1)
                        .Any(m => String.Equals(m, candidate.DbName, StringComparison.InvariantCultureIgnoreCase))
                    );
        }

        private static string[] TableKeyMatchList(Table table)
        {
            return new[]
            {
                "Id",
                table.ControllerType.Name + "Id",
                table.ControllerType.Name + "_Id",
                table.DbName + "Id",
                table.DbName + "_Id"
            };
        }
    }
}