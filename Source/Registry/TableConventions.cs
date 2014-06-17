using System;
using System.Linq;
using Pagan.Relationships;

namespace Pagan.Registry
{
    public class TableConventions: ITableConventions
    {
        /// <summary>
        /// This method is called during Table conventions to provide a DbName for the default schema where
        /// this property has not been explicitly set
        /// </summary>
        /// <returns>The default schema name</returns>
        public string GetSchemaDbName(Table table)
        {
            return "dbo";
        }

        public string GetTableDbName(Table table)
        {
            return table.Name;
        }

        /// <summary>
        /// This method is called during Table conventions to provide a DbName for columns where this property
        /// has not been explicitly set. 
        /// By default, DbName is set to match the name of the property on the Table.
        /// </summary>
        /// <param name="column">The column to be configured</param>
        public string GetColumnDbName(Column column)
        {
            return column.Name;
        }

        /// <summary>
        /// This method is called during Table conventions to attempt to set the primary key for a table
        /// where the key has not been explicitly set.
        /// Attempts to match, in order, "Id", "{ControllerName}Id", "{ControllerName}_Id", "{TableName}Id", "{TableName}_Id"
        /// </summary>
        /// <param name="table">the table to configure</param>
        public Column[] GetPrimaryKey(Table table)
        {
            Column column;
            return
                table.TryGetColumn("Id", out column)
                || table.TryGetColumn(table.Name + "Id", out column)
                || table.TryGetColumn(table.Name + "_Id", out column)
                || table.TryGetColumn(table.DbName + "Id", out column)
                || table.TryGetColumn(table.DbName + "_Id", out column)
                    ? new[] {column}
                    : new Column[0];
        }

        /// <summary>
        /// This method is called when a related table is joined to attempt set the foreign key for a table
        /// where the key has not been explicitly set.
        /// </summary>
        /// <param name="dependent">the dependent to configure</param>
        /// <param name="columns">the available columns</param>
        public void SetDefaultForeignKey(IDependent dependent, Column[] columns)
        {
            var principal = dependent.GetPrincipal();

            var fk = principal
                .PrimaryKeyColumns
                .Select(keyColumn => columns.FirstOrDefault(column => Match(keyColumn, column)))
                .Where(column => !ReferenceEquals(null, column))
                .ToArray();
            
            if(fk.Length==principal.PrimaryKeyColumns.Length) dependent.SetForeignKey(fk);
        }

        /// <summary>
        /// Helper method for SetDefaultForeignKey. Attempts to matche a key column name that starts with
        /// the key table name, or the key name prefixed with the key table name. 
        /// <example> A foreign key column "BlogId" would match a primary key of BlogId or Id on the principal table "Blog"</example>
        /// </summary>
        /// <param name="keyColumn">A primary key column</param>
        /// <param name="candidate">A foreign key candidate</param>
        /// <returns>True if matched, else false </returns>
        private static bool Match(Column keyColumn, Column candidate)
        {
            var fkName =
                keyColumn.DbName.StartsWith(keyColumn.Table.DbName, StringComparison.InvariantCultureIgnoreCase)
                    ? keyColumn.Name
                    : keyColumn.Table.DbName + keyColumn.DbName;

            return string.Equals(candidate.DbName, fkName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}