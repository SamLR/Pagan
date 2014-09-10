using System;
using System.Collections.Generic;
using System.Linq;
using Pagan.Conditions;
using Pagan.Configuration;
using Pagan.SqlObjects;

namespace Pagan.Relationships
{
    public abstract class Relationship<T>: Relationship
    {
        protected readonly Dictionary<Field, Func<T, Field>> Mappings;
        private readonly Lazy<Definition<T>> _other;
        private readonly Lazy<Relationship> _twin;

        internal Relationship(string memberName, Definition definition) : base(memberName, definition)
        {
            Mappings = new Dictionary<Field, Func<T, Field>>();
            _other = new Lazy<Definition<T>>(GetOther);
            _twin = new Lazy<Relationship>(GetTwin);
        }

        private Definition<T> GetOther()
        {
            return Definition.Factory.GetDefinition<T>();
        }

        private Relationship GetTwin()
        {
            var me = typeof(Relationship<>).MakeGenericType(Definition.Type);
            var twin = Other.Relationships.FirstOrDefault(me.IsInstanceOfType);

            if (twin == null)
                throw RelationshipError.MissingTwin(typeof(T), MemberName, Definition.Type);

            return twin;
        }

        private Condition GetConditionFromTwin()
        {
            if (!Twin.HasMappings)
                throw RelationshipError.NoMappingDefined(MemberName, Twin.MemberName);

            return Twin.GetCondition();
        }

        internal Definition<T> Other
        {
            get { return _other.Value; }
        }

        private Relationship Twin
        {
            get { return _twin.Value; }
        }

        internal override bool HasMappings
        {
            get { return Mappings.Count > 0; }
        }

        internal override Condition GetCondition()
        {
            // field mappings can be defined on either side of the relationship
            return !HasMappings 
                ? GetConditionFromTwin() 
                : LogicalGroup.And(GetFieldMatchConditions());
        }

        private RelationshipType GetRelationshipType()
        {
            if (Type.HasValue) return Type.Value;

            if (!Twin.Type.HasValue)
                throw RelationshipError.NoRelationshipTypeDefined(MemberName, Twin.MemberName);

            return Twin.Type.Value;
        }

        public override JoinedTable GetJoin()
        {
            return new JoinedTable
            {
                JoinCondition = GetCondition(),
                Role = Role,
                Type = GetRelationshipType(),
                Table = Other.Table
            };
        }
    }
}