using System;

namespace Pagan.Relationships
{
    public class RelationshipError : Exception
    {
        private RelationshipError(string message, params object[] args) : base(String.Format(message, args))
        {
        }

        internal static RelationshipError NoMappingDefined(Type principal, Type dependent)
        {
            return new RelationshipError(
                "No mapping between '{0}' and '{1}' was defined on either end of the relationship",
                principal,
                dependent
                );
        }

        internal static RelationshipError NoRelationshipDefined(Type left, Type right)
        {
            return new RelationshipError(
                "No relationship of type '{1}' was defined on the definition for type '{0}'",
                left,
                right
                );
        }
    }
}