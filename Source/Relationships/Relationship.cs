using System.Collections.Generic;
using Pagan.Conditions;
using Pagan.Configuration;
using Pagan.SqlObjects;

namespace Pagan.Relationships
{
    public abstract class Relationship : DefinitionItem
    {
        internal Relationship(string memberName, Definition definition): base(memberName, definition)
        {
        }

        internal RelationshipType? Type; 
        internal abstract IEnumerable<FieldMatchCondition> GetFieldMatchConditions();
        internal abstract bool HasMappings { get; }
        internal abstract Condition GetCondition();
        internal abstract RelationshipRole Role { get; }
        public abstract JoinedTable GetJoin();

        public void HasMany()
        {
            Type = RelationshipType.HasMany;
        }

        public void HasOne()
        {
            Type = RelationshipType.HasOne;
        }

        public void WithMany()
        {
            Type = RelationshipType.WithMany;
        }

        public void WithOne()
        {
            Type = RelationshipType.WithOne;
        }
    }
}
