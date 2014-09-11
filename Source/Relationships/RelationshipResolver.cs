using System;
using System.Linq;
using Pagan.Conditions;
using Pagan.Configuration;
using Pagan.SqlObjects;

namespace Pagan.Relationships
{
    class RelationshipResolver
    {
        private readonly IDefinitionFactory _factory;

        public RelationshipResolver(IDefinitionFactory factory)
        {
            _factory = factory;
        }

        public JoinedTable GetJoin(Type leftType, Type rightType)
        {
            var left = _factory.GetDefinition(leftType);
            var right = _factory.GetDefinition(rightType);

            return GetJoin(left, right);
        }

        public JoinedTable GetJoin(IDefinition left, IDefinition right)
        {
            var leftEnd = GetRelationshipBetween(left, right);

            if (leftEnd == null)
                throw RelationshipError.NoRelationshipDefined(left.Type, right.Type);

            Condition joinCondition;

            if (leftEnd.HasMappings) joinCondition = leftEnd.CreateJoin(right.Instance);
            else
            {
                var rightEnd = GetRelationshipBetween(right, left);

                if (rightEnd == null || !rightEnd.HasMappings)
                    throw RelationshipError.NoMappingDefined(left.Type, right.Type);

                joinCondition = rightEnd.CreateJoin(left.Instance);
            }

            return new JoinedTable
            {
                End = leftEnd.RelatesTo,
                JoinCondition = joinCondition,
                Multiplicity = leftEnd.Multiplicity,
                Table = right.Table
            };
        }

        public Relationship GetRelationshipBetween(IDefinition left, IDefinition right)
        {
            return left.Relationships.FirstOrDefault(x => x.RelatedType == right.Type);
        }
    }
}
