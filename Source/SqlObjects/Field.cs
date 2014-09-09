using Pagan.Configuration;

namespace Pagan.SqlObjects
{
    public class Field: DefinitionItem
    {
        internal Field(string memberName, Definition definition) : base(memberName, definition)
        {
        }

        public Table Table { get; internal set; }

        public bool IsKey { get; protected set; }
    }
}