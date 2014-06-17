using System;
using Pagan.Registry;

namespace Pagan.Relationships
{
    public abstract class LinkRef
    {
        protected LinkRef(Table table, string name)
        {
            Table = table;
            Name = name;
        }

        protected internal Type PartnerControllerType;
        protected internal Table Table;

        protected internal void EnsureForeignKey()
        {
            var dependent = (IDependent)this;
            
            if (dependent.HasForeignKey()) return;

            Table.Conventions.SetDefaultForeignKey(dependent, Table.Columns);

            if (!dependent.HasForeignKey())
                throw ConfigurationError.MissingForeignKey(Table.ControllerType);
        }

        public string Name { get; protected set; }
    }
}
