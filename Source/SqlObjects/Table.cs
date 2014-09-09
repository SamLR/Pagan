using Pagan.Configuration;

namespace Pagan.SqlObjects
{
    public class Table: DefinitionItem
    {
        internal Table(string memberName, Definition definition) : base(memberName, definition)
        {
        }

        public Schema Schema { get; internal set; }

        public void UseSingularForm()
        {
            Name = Definition.Type.Name;
        }
    }
}
