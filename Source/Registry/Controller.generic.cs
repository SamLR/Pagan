using System;
using System.Linq;
using System.Reflection;
using Pagan.DbComponents;
using Pagan.Relationships;

namespace Pagan.Registry
{
    public class Controller<T>: Controller
    {
        internal Controller(IControllerFactory factory, IDbConfiguration configuration): base(factory, configuration)
        {
            ControllerType = typeof (T);
            Instance = Activator.CreateInstance<T>();
            Configure();
        }

        public T Instance { get; private set; }

        private void Configure()
        {
            var members =
                ControllerType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite)
                    .ToArray();

            Table = CreateMember<Table>(members.FirstOrDefault(m => m.PropertyType == typeof (Table)));

            Schema = CreateMember<Schema>(members.FirstOrDefault(m => m.PropertyType == typeof(Schema)));

            Columns = members
                .Where(m => m.PropertyType == typeof (Column))
                .Select(CreateMember<Column>)
                .ToArray();

            LinkRefs = members
                .Where(m => typeof (LinkRef).IsAssignableFrom(m.PropertyType))
                .Select(CreateMember<LinkRef>)
                .ToArray();


            // throw if no table
            if (Table == null) throw ConfigurationError.MissingTable(ControllerType);

            // throw if no columns
            if (Columns.Length == 0) throw ConfigurationError.MissingColumns(ControllerType);

            // call "Configure" method on controller if present
            var method = ControllerType.GetMethod("Configure", BindingFlags.Public | BindingFlags.Instance);
            if (method != null)
            {
                method.Invoke(Instance, null);
            }

            // use the default schema if none was defined
            if (Schema == null) Schema = new Schema(this, Configuration.GetDefaultSchemaName());

            // set the DbName for columns where none was explicitly defined.
            Columns.Where(c => String.IsNullOrEmpty(c.DbName)).ForEach(Configuration.SetDefaultColumnDbName);

            // attempt to set a default primary key where none was explicitly defined
            if (!this.HasPrimaryKey()) Configuration.SetDefaultPrimaryKey(Columns);

            // throw if still no primary key
            if (!this.HasPrimaryKey()) throw ConfigurationError.MissingKey(ControllerType);

        }

        private TMember CreateMember<TMember>(PropertyInfo property)
        {
            if (ReferenceEquals(null, property)) return default(TMember);

            var dbComponent = Activator.CreateInstance(property.PropertyType, this, property.Name);
            property.SetValue(Instance, dbComponent);
            return (TMember)dbComponent;
        }
    }
}