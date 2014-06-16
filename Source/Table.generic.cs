using System;
using System.Linq;
using System.Reflection;
using Pagan.Registry;
using Pagan.Relationships;

namespace Pagan
{
    public class Table<T>: Table
    {
        internal Table(ITableFactory factory, ITableConfiguration configuration): base(factory, configuration)
        {
            ControllerType = typeof (T);
            Controller = Activator.CreateInstance<T>();
            Configure();
        }

        public T Controller { get; private set; }

        private void Configure()
        {
            var members =
                ControllerType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite)
                    .ToArray();

            var table = members.FirstOrDefault(m => m.PropertyType == typeof(Table));

            // throw if no table
            if (table == null) throw ConfigurationError.MissingTable(ControllerType);

            // check for attribute configuration. [UseAsTableName] on the controller or [DbName] on the table.
            var dbName = table.GetCustomAttribute<DbNameAttribute>();
            var useAsTable = ControllerType.GetCustomAttribute<UseAsTableNameAttribute>();

            DbName = dbName != null ? dbName.Value : useAsTable != null ? ControllerType.Name : Name;
            Name = ControllerType.Name;
            table.SetValue(Controller, this);

            // use the default schema if none was defined
            Schema = CreateMember<Schema>(members.FirstOrDefault(m => m.PropertyType == typeof (Schema))) ??
                     new Schema(this, Configuration.GetDefaultSchemaName());
            
            Columns = members
                .Where(m => m.PropertyType == typeof (Column))
                .Select(CreateColumn)
                .ToArray();

            // throw if no columns
            if (Columns.Length == 0) throw ConfigurationError.MissingColumns(ControllerType);

            LinkRefs = members
                .Where(m => typeof (LinkRef).IsAssignableFrom(m.PropertyType))
                .Select(CreateMember<LinkRef>)
                .ToArray();
            
            // call "Configure" method on Controller if defined
            var method = ControllerType.GetMethod("Configure", BindingFlags.Public | BindingFlags.Instance);
            if (method != null)
            {
                method.Invoke(Controller, null);
            }

            // attempt to set a default primary key where none was explicitly defined
            if (!this.HasPrimaryKey()) Configuration.SetDefaultPrimaryKey(this);

            // throw if still no primary key
            if (!this.HasPrimaryKey()) throw ConfigurationError.MissingKey(ControllerType);

        }

        private Column CreateColumn(PropertyInfo property)
        {
            var column = CreateMember<Column>(property);
            var dbName = property.GetCustomAttribute<DbNameAttribute>();
            column.DbName = (dbName != null) ? dbName.Value : column.Name;
            return column;
        }

        private TMember CreateMember<TMember>(PropertyInfo property)
        {
            if (property == null) return default(TMember);

            var dbComponent = Activator.CreateInstance(property.PropertyType, this, property.Name);
            property.SetValue(Controller, dbComponent);
            return (TMember)dbComponent;
        }
    }
}