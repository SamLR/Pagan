using System;
using System.Collections.Generic;
using System.Linq;
using Pagan.Conditions;
using Pagan.Configuration;
using Pagan.SqlObjects;

namespace Pagan.Relationships
{
    public abstract class Relationship : IDefinitionItem
    {
        protected Relationship(string name)
        {
            Name = name;
        }

        internal abstract bool HasMappings { get; }
        internal abstract Condition CreateJoin(object related);

        public Type RelatedType { get; protected set; }
        public Type DefiningType { get; internal set; }
        public RelationshipEnd RelatesTo { get; protected set; }
        public string Name { get; private set; }
        public Multiplicity Multiplicity { get; protected set; }
    }

    public abstract class Relationship<T> : Relationship where T:class
    {
        protected Relationship(string name) : base(name)
        {
            RelatedType = typeof (T);
            Mappings = new Dictionary<Field, Func<T, Field>>();
        }

        protected Dictionary<Field, Func<T, Field>> Mappings;
        protected abstract IEnumerable<FieldJoin> GetFieldJoins(T related);

        internal override bool HasMappings { get { return Mappings.Count > 0; } }
        internal override Condition CreateJoin(object related)
        {
            var target = (T) related;

            return Mappings.Count > 1
                ? (Condition) LogicalGroup.And(GetFieldJoins(target))
                : GetFieldJoins(target).First();
        }
    }
}
