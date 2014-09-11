using System.Linq;
using System.Reflection;
using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Configuration
{
    internal partial class Definition
    {
        private void CreateMembers()
        {
            var members = Type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .Where(p => typeof(IDefinitionItem).IsAssignableFrom(p.PropertyType));

            foreach (var p in members)
            {
                var ctor = p.PropertyType
                    .GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                        null, new[] {typeof (string)}, null);

                var item = ctor.Invoke(new object[] { p.Name });

                p.SetValue(Instance, item);

                var schema = item as Schema;
                if (schema != null) Schema = schema;

                var table = item as Table;
                if (table != null) Table = table;

                var field = item as Field;
                if (!ReferenceEquals(field, null)) Fields.Add(field);

                var key = item as Key;
                if (!ReferenceEquals(key, null)) Keys.Add(key);

                var relationship = item as Relationship;
                if (relationship != null)
                {
                    Relationships.Add(relationship);
                    relationship.DefiningType = Type;
                }
            }

            if (Fields.Count == 0)
                throw DefinitionError.MissingDefinitionMember<Field>(this);

            if (Keys.Count == 0)
                throw DefinitionError.MissingDefinitionMember<Key>(this);

            if (Table == null) Table = new Table(Type.Name);

            Table.Schema = Schema;

            Fields.ForEach(f => f.Table = Table);

        }

        private void CallConfigure()
        {
            var configure = Type.GetMethod("Configure", BindingFlags.Instance | BindingFlags.Public);

            if (configure == null) return;

            configure.Invoke(Instance, null);
        }
    }
}