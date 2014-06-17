using System;
using System.Linq;
using Pagan.Registry;
using Pagan.Relationships;

namespace Pagan
{
    public abstract class Table
    {
        protected Table(ITableFactory factory, ITableConventions conventions)
        {
            Factory = factory;
            Conventions = conventions;
        }
        
        public string Name { get; internal set; }
        public string DbName { get; internal set; }
        public Type ControllerType { get; protected set; }
        
        public Schema Schema { get; internal set; }
        public Column[] Columns { get; internal set; }
        public Column[] KeyColumns { get; internal set; }
        public LinkRef[] LinkRefs { get; internal set; }

        public void SetKey(params Column[] keyColumns)
        {
            KeyColumns = keyColumns;
        }
        
        internal ITableFactory Factory;
        internal ITableConventions Conventions;

        public LinkRef GetLinkRef(Type type)
        {
            return LinkRefs.First(r => r.PartnerControllerType == type);
        }

        public bool TryGetColumn(string name, out Column column)
        {
            column = Columns.FirstOrDefault(c => String.Equals(c.Name, name, StringComparison.InvariantCultureIgnoreCase));
            return !ReferenceEquals(null, column);
        }
    }
}