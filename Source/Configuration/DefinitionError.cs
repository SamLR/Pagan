using System;

namespace Pagan.Configuration
{
    public class DefinitionError: Exception
    {
        private DefinitionError(string message, params object[] args) : base(String.Format(message, args))
        {
        }

        internal static DefinitionError MissingDefinitionMember<T, TDef>()
        {
            return new DefinitionError("No member of type '{0}' is defined on type '{1}", typeof (T), typeof (TDef));
        } 
    }
}