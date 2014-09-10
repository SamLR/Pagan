using System;
using System.Collections.Generic;
using System.Linq;
using Pagan.Conditions;
using Pagan.Configuration;
using Pagan.SqlObjects;

namespace Pagan.Relationships
{
    public class Dependent<T> : Relationship<T>
    {
        internal Dependent(string memberName, Definition definition) : base(memberName, definition)
        {
        }

        public Dependent<T> Map(Field fKey, Func<T, Key> keySelector)
        {
            Mappings[fKey] = keySelector;
            return this;
        }

        internal override IEnumerable<FieldMatchCondition> GetFieldMatchConditions()
        {
            return Mappings.Select(p => new FieldMatchCondition(p.Value(Other.Instance), p.Key));
        }

        internal override RelationshipRole Role
        {
            get { return RelationshipRole.Principal; }
        }
    }
}