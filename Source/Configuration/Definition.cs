using System;
using System.Collections.Generic;
using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Configuration
{
    internal abstract class Definition
    {
        protected Definition(IDefinitionFactory factory)
        {
            Fields = new List<Field>();
            Keys = new List<Key>();
            Relationships = new List<Relationship>();
            Factory = factory;
        }

        internal IDefinitionFactory Factory;
        public Type Type { get; protected set; }
        public Schema Schema { get; internal set; }
        public Table Table { get; internal set; }
        public List<Field> Fields { get; private set; }
        public List<Key> Keys { get; private set; }
        public List<Relationship> Relationships { get; private set; } 
    }
}