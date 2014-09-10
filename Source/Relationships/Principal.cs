using System;
using System.Collections.Generic;
using System.Linq;
using Pagan.Conditions;
using Pagan.Configuration;
using Pagan.SqlObjects;

namespace Pagan.Relationships
{
    public class Principal<T> : Relationship<T>
    {
        internal Principal(string memberName, Definition definition) : base(memberName, definition)
        {
        }

        public Principal<T> Map(Key key, Func<T, Field> fkSelector)
        {
            Mappings[key] = fkSelector;
            return this;
        }

        internal override IEnumerable<FieldMatchCondition> GetFieldMatchConditions()
        {
            return Mappings.Select(p => new FieldMatchCondition(p.Key, p.Value(Other.Instance)));
        }

        internal override RelationshipRole Role
        {
            get { return RelationshipRole.Dependent; }
        }
    }
}