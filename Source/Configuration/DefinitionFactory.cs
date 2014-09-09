using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pagan.SqlObjects;

namespace Pagan.Configuration
{
    internal class DefinitionFactory: IDefinitionFactory
    {
        private readonly Dictionary<Type, Definition> _cache;

        internal DefinitionFactory()
        {
            _cache = new Dictionary<Type, Definition>();
        }

        public Definition<T> GetDefinition<T>()
        {
            var definitionType = typeof (T);

            Definition definition;
            
            if (!_cache.TryGetValue(definitionType, out definition))
                _cache[definitionType] = definition = CreateDefinition<T>();
            
            return (Definition<T>) definition;
        }

        private Definition<T> CreateDefinition<T>()
        {
            var definition = new Definition<T>(this);

            CreateMembers(definition);

            if (definition.Table == null)
                throw DefinitionError.MissingDefinitionMember<Table, T>();

            if(definition.Fields.Count==0)
                throw DefinitionError.MissingDefinitionMember<Field, T>();

            if (definition.Keys.Count == 0)
                throw DefinitionError.MissingDefinitionMember<Key, T>();


            definition.Table.Schema = definition.Schema;

            definition.Fields.ForEach(f => f.Table = definition.Table);

            CallConfigure(definition);

            return definition;
        }

        private static void CreateMembers<T>(Definition<T> definition)
        {
            var members = definition.Type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .Where(p => typeof (DefinitionItem).IsAssignableFrom(p.PropertyType));

            foreach (var p in members)
            {
                var ctor = p.PropertyType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { typeof(string), typeof(Definition) }, null);
                var item = ctor.Invoke(new object[] {p.Name, definition});

                p.SetValue(definition.Instance, item);

                var schema = item as Schema;
                if (schema != null) definition.Schema = schema;

                var table = item as Table;
                if (table != null) definition.Table = table;

                var field = item as Field;
                if (!ReferenceEquals(field,null)) definition.Fields.Add(field);

                var key = item as Key;
                if (!ReferenceEquals(key, null)) definition.Keys.Add(key);
            }

        }

        private static void CallConfigure<T>(Definition<T> definition)
        {
            var configure = definition.Type.GetMethod("Configure", BindingFlags.Instance | BindingFlags.Public);
            
            if (configure == null) return;
            
            configure.Invoke(definition.Instance, null);
        }
    }
}