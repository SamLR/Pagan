using System;
using Pagan.Registry;

namespace Pagan.Relationships
{
    public abstract class LinkRef
    {
        protected LinkRef(Controller controller, string name)
        {
            Controller = controller;
            Name = name;
        }

        protected internal Type PartnerControllerType;
        protected internal Controller Controller;

        protected internal void EnsureForeignKey()
        {
            var dependent = (IDependent)this;
            
            if (dependent.HasForeignKey()) return;

            Controller.Configuration.SetDefaultForeignKey(dependent, Controller.Columns);

            if (!dependent.HasForeignKey())
                throw ConfigurationError.MissingForeignKey(Controller.ControllerType);
        }

        public string Name { get; protected set; }
    }
}
