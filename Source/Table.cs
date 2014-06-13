using System;
using System.Linq;
using Pagan.Registry;
using Pagan.Relationships;

namespace Pagan
{
    public abstract class Table
    {
        protected Table(ITableFactory factory, ITableConfiguration configuration)
        {
            Factory = factory;
            Configuration = configuration;
        }
        
        public string Name { get; protected set; }
        public string DbName { get; protected set; }
        public Type ControllerType { get; protected set; }
        
        public Schema Schema { get; protected set; }
        public Column[] Columns { get; protected set; }
        public Column[] KeyColumns { get; protected internal set; }
        public LinkRef[] LinkRefs { get; protected set; }

        public void SetKey(params Column[] keyColumns)
        {
            KeyColumns = keyColumns;
        }
        
        internal ITableFactory Factory;
        internal ITableConfiguration Configuration;

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