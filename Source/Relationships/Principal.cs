using System;
using System.Collections.Generic;
using System.Linq;
using Pagan.Conditions;
using Pagan.SqlObjects;

namespace Pagan.Relationships
{
    public abstract class Principal<T> : Relationship<T>
    {
        protected Principal(string name): base(name)
        {
            RelatesTo = RelationshipEnd.Dependent;
        }

        public Principal<T> Map(Field foreignKey, Func<T, Key> key)
        {
            Mappings[foreignKey] = key;
            return this;
        }

        protected override IEnumerable<FieldJoin> GetFieldJoins(T related)
        {
            return Mappings.Select(m => new FieldJoin(m.Value(related), m.Key));
        }
    }
}