using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pagan.SqlObjects;

namespace Pagan.Metadata
{
    internal class TableFactory : ITableFactory
    {
        private readonly Dictionary<Type, Table> _maps;

        public TableFactory()
        {
            _maps = new Dictionary<Type, Table>();
        }

        public Table GetTable(Type entityType)
        {
            Table info;
            if (_maps.TryGetValue(entityType, out info)) return info;

            _maps[entityType] = info = CreateTable(entityType);
            
            return info;
        }

        public Table GetTable<T>()
        {
            return GetTable(typeof (T));
        }

        private static Table CreateTable(Type entityType)
        {
            var tableNameAttr = entityType.GetCustomAttribute<TableNameAttribute>();
            var schemaNameAttr = entityType.GetCustomAttribute<SchemaNameAttribute>();
            var fields =
                entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var table = new Table
            {
                SqlTable = new SqlTable
                {
                    Name = tableNameAttr == null ? entityType.Name : tableNameAttr.Name,
                    Schema = schemaNameAttr == null ? null : schemaNameAttr.Name
                },
                EntityType = entityType,
            };

            table.Fields = fields.Select(f => CreateField(f, table)).ToArray();
            CreatePrimaryKey(table);

            return table;
        }

        private static Field CreateField(PropertyInfo propertyInfo, Table table)
        {
            var fieldNameAttr = propertyInfo.GetCustomAttribute<FieldNameAttribute>();

            return new Field
            {
                SqlField = new SqlField
                {
                    Name = fieldNameAttr == null ? propertyInfo.Name : fieldNameAttr.Name,
                    Table = table.SqlTable
                },
                PropertyInfo = propertyInfo,
                Table = table
            };
        }

        private static void CreatePrimaryKey(Table table)
        {
            var entityName = table.EntityType.Name;
            var tableName = table.SqlTable.Name;
            var list = new[]
            {
                "Id",
                entityName + "Id",
                entityName + "_Id",
                tableName + "Id",
                tableName + "_Id",
            };
            
            var pk = table.SelectField(list);

            if (pk != null)
                pk.IsKey = true;
        }
    }
}
