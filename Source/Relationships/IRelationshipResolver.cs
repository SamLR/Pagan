using System;
using Pagan.Configuration;
using Pagan.SqlObjects;

namespace Pagan.Relationships
{
    internal interface IRelationshipResolver
    {
        JoinedTable GetJoin(Relationship relationship);
        JoinedTable GetJoin(Type leftType, Type rightType);
        JoinedTable GetJoin(IDefinition left, IDefinition right);
        Relationship GetRelationshipBetween(IDefinition left, IDefinition right);
    }
}