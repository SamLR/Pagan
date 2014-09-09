using System;
using System.Collections.Generic;
using System.Linq;
using Pagan.Conditions;
using Pagan.Configuration;
using Pagan.SqlObjects;

namespace Pagan.Relationships
{
    public abstract class Relationship<T>: DefinitionItem
    {
        protected readonly Dictionary<Field, Func<T, Field>> Mappings;

        internal Relationship(string memberName, Definition definition) : base(memberName, definition)
        {
            Mappings = new Dictionary<Field, Func<T, Field>>();
        }

        internal Condition GetCondition()
        {
            var other = Definition.Factory.GetDefinition<T>();
            return LogicalGroup.And(
                Mappings.Select(p => new FieldMatchCondition(p.Key, p.Value(other.Instance)))
                );
        }
    }
}