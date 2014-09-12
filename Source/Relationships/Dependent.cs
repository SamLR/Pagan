using System;
using System.Collections.Generic;
using System.Linq;
using Pagan.Conditions;
using Pagan.SqlObjects;

namespace Pagan.Relationships
{
    public abstract class Dependent<T> : Relationship<T> where T:class,new()
    {
        protected Dependent(string name) : base(name)
        {
            RelatesTo = RelationshipEnd.Principal;
        }

        public Dependent<T> Map(Field foreignKey, Func<T, Key> key)
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