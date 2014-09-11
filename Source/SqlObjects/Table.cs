using Pagan.Configuration;

namespace Pagan.SqlObjects
{
    public class Table: IDefinitionItem
    {
        internal Table(string name)
        {
            Name = name;
        }

        public Schema Schema { get; internal set; }
        public string Name { get; private set; }
    }
}
