using System;
using System.Linq;
using Pagan.DbComponents;
using Pagan.Relationships;

namespace Pagan.Registry
{
    public abstract class Controller
    {
        protected Controller(IControllerFactory factory, IDbConfiguration configuration)
        {
            Factory = factory;
            Configuration = configuration;
        }

        public Type ControllerType { get; protected set; }
        public Schema Schema { get; protected set; }
        public Table Table { get; protected set; }
        public Column[] Columns { get; protected set; }
        public Column[] KeyColumns { get; protected internal set; }
        public LinkRef[] LinkRefs { get; protected set; }

        protected internal IControllerFactory Factory;
        protected internal IDbConfiguration Configuration;

        public LinkRef GetLinkRef(Type type)
        {
            return LinkRefs.First(r => r.PartnerControllerType == type);
        }
    }
}