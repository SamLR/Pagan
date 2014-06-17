using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pagan.Registry;
using Pagan.Relationships;

namespace Pagan
{
    public class Table<T>: Table
    {
        internal Table(ITableFactory factory, ITableConventions conventions): base(factory, conventions)
        {
            ControllerType = typeof (T);
            Controller = Activator.CreateInstance<T>();
            Configure();
        }

        public T Controller { get; private set; }

        private void Configure()
        {
            var members = ControllerType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToArray();


            ConfigureTable(members.FirstOrDefault(m => m.PropertyType == typeof(Table)));
            ConfigureSchema(members.FirstOrDefault(m => m.PropertyType == typeof(Schema)));
            ConfigureColumns(members.Where(m => m.PropertyType == typeof(Column)));
            ConfigureLinkRefs(members.Where(m => typeof(LinkRef).IsAssignableFrom(m.PropertyType)));

            CallControllerConfiguration();

            if (!this.HasPrimaryKey()) SetKey(Conventions.GetPrimaryKey(this));

            // throw if still no primary key
            if (!this.HasPrimaryKey()) throw ConfigurationError.MissingKey(ControllerType);
        }

        private void ConfigureTable(PropertyInfo property)
        {
            // throw if no setthis this property defined by controller
            if (property == null) throw ConfigurationError.MissingTable(ControllerType);

            Name = property.Name;

            // check for this attribute conventions. [UseAsthisName] on the controller or [DbName] on the this.
            var dbName = property.GetCustomAttribute<DbNameAttribute>();
            var useAsTable = ControllerType.GetCustomAttribute<UseAsTableNameAttribute>();

            DbName = dbName != null
                ? dbName.Value
                : useAsTable != null
                    ? ControllerType.Name
                    : Conventions.GetTableDbName(this);

            // set this property on controller
            property.SetValue(Controller, this);
        }

        private void ConfigureSchema(PropertyInfo property)
        {
            Schema = property != null
                ? CreateMember<Schema>(property)
                : new Schema(this, Conventions.GetSchemaDbName(this));
        }

        private void ConfigureColumns(IEnumerable<PropertyInfo> properties)
        {
            Columns = properties
                .Select(property =>
                {
                    var column = CreateMember<Column>(property);
                    var dbName = property.GetCustomAttribute<DbNameAttribute>();
                    column.DbName = dbName != null ? dbName.Value : Conventions.GetColumnDbName(column);
                    return column;
                })
                .ToArray();

            // throw if no columns
            if (Columns.Length == 0) throw ConfigurationError.MissingColumns(ControllerType);
        }

        private TMember CreateMember<TMember>(PropertyInfo property)
        {
            var dbComponent = Activator.CreateInstance(property.PropertyType, this, property.Name);
            property.SetValue(Controller, dbComponent);
            return (TMember)dbComponent;
        }

        private void ConfigureLinkRefs(IEnumerable<PropertyInfo> properties)
        {
            LinkRefs = properties
                .Select(CreateMember<LinkRef>)
                .ToArray();
        }

        private void CallControllerConfiguration()
        {
            // call "Configure" method on Controller if defined
            var method = ControllerType.GetMethod("Configure", BindingFlags.Public | BindingFlags.Instance);
            if (method != null)
            {
                method.Invoke(Controller, null);
            }
        }
    }
}