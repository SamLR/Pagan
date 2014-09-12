using System;

namespace Pagan.Relationships
{
    public interface IRelationship
    {
        Type RelatedType { get; }
        Type DefiningType { get; }
        RelationshipEnd RelatesTo { get; }
        string Name { get; }
        Multiplicity Multiplicity { get; }
    }
}