using System;

namespace Pagan.Relationships
{
    public class RelationshipError : Exception
    {
        private RelationshipError(string message, params object[] args) : base(String.Format(message, args))
        {
        }

        internal static RelationshipError NoMappingDefined(string principal, string dependent)
        {
            return new RelationshipError(
                "No mapping for keys on {0} to fields on {1} was defined",
                principal,
                dependent
                );
        }

        internal static RelationshipError NoRelationshipTypeDefined(string principal, string dependent)
        {
            return new RelationshipError(
                "No relationship type was defined on either {0} or {1} was defined",
                principal,
                dependent
                );
        }

        internal static RelationshipError MissingTwin(Type other, string name, Type me)
        {
            return new RelationshipError(
                "No twin relationship is defined on {0} that matches relationship {1} defined on {2} ",
                other,
                name,
                me
                );
        }
    }
}