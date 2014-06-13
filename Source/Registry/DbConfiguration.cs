using System;
using System.Linq;
using Pagan.DbComponents;
using Pagan.Relationships;

namespace Pagan.Registry
{
    public class DbConfiguration: IDbConfiguration
    {
        /// <summary>
        /// This method is called during controller configuration to provide a DbName for the default schema where
        /// this property has not been explicitly set
        /// </summary>
        /// <returns>The default schema name</returns>
        public string GetDefaultSchemaName()
        {
            return "dbo";
        }

        /// <summary>
        /// This method is called during controller configuration to provide a DbName for columns where this property
        /// has not been explicitly set. 
        /// By default, DbName is set to match the name of the property on the controller.
        /// </summary>
        /// <param name="column">The column to be configured</param>
        public void SetDefaultColumnDbName(Column column)
        {
            column.DbName = column.Name;
        }

        /// <summary>
        /// This method is called during controller configuration to attempt to set the primary key for a table
        /// where the key has not been explicitly set.
        /// By default, the key is set to the first column that matches "Id" or "{TableName}Id"
        /// </summary>
        /// <param name="columns">the available columns</param>
        public void SetDefaultPrimaryKey(Column[] columns)
        {
            var idColumn = columns
                .FirstOrDefault(c =>
                    String.Equals("Id", c.Name, StringComparison.InvariantCultureIgnoreCase)
                    || String.Equals(c.Table.DbName + "Id", c.DbName, StringComparison.InvariantCultureIgnoreCase));

            if(!ReferenceEquals(null,idColumn))
                idColumn.Table.SetKeys(idColumn);
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