using System;
using System.Collections.Generic;
using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Configuration
{
    internal partial class Definition: IDefinition
    {
        internal Definition(Type definitionType)
        {
            Type = definitionType;
            Instance = Activator.CreateInstance(definitionType);
            Fields = new List<Field>();
            Keys = new List<Key>();
            Relationships = new List<Relationship>();

            CreateMembers();
            CallConfigure();
        }

        public Type Type { get; internal set; }
        public object Instance { get; internal set; }
        public Schema Schema { get; internal set; }
        public Table Table { get; internal set; }
        public List<Field> Fields { get; private set; }
        public List<Key> Keys { get; private set; }
        public List<Relationship> Relationships { get; private set; } 
    }
}