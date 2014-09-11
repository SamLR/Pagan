using Pagan.Configuration;

namespace Pagan.SqlObjects
{
    public class Schema: IDefinitionItem
    {
        internal Schema(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}