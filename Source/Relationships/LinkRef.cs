using System;
using Pagan.Queries;
using Pagan.Registry;

namespace Pagan.Relationships
{
    public abstract class LinkRef: ILinkRef
    {
        protected LinkRef(Table table, string name)
        {
            Table = table;
            Name = name;
        }

        protected internal Type PartnerControllerType;

        protected void EnsureForeignKey(IPrincipal principal, IDependent dependent)
        {
            if (dependent.HasForeignKey()) return;

            dependent.SetForeignKey(Table.Conventions.GetForeignKey(principal.Table, dependent.Table));

            if (!dependent.HasForeignKey())
                throw ConfigurationError.MissingForeignKey(dependent.Table.ControllerType);
        }

        public abstract Query Query();
        public Table Table { get; private set; }
        public string Name { get; protected set; }
    }
}
