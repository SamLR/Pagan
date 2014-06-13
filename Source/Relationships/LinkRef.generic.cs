using System;
using Pagan.Queries;
using Pagan.Registry;

namespace Pagan.Relationships
{
    public abstract class LinkRef<T>: LinkRef
    {
        protected LinkRef(Controller controller, string name): base(controller, name)
        {
            PartnerControllerType = typeof (T);
        }

        private Controller<T> GetPartnerController()
        {
            return Controller.Factory.GetController<T>();
        }

        protected LinkRef GetPartnerRef()
        {
            return GetPartnerController().GetLinkRef(Controller.ControllerType);
        }

        protected Relationship GetRelationship()
        {
            var linkRef = GetPartnerRef();

            var dependent = linkRef as IDependent;
            if (dependent != null)
            {
                linkRef.EnsureForeignKey();
                return new Relationship((IPrincipal) this, dependent, Role.Dependent);
            }

            EnsureForeignKey();
            return new Relationship((IPrincipal) linkRef, (IDependent) this, Role.Principal);
        }

        public Query Query()
        {
            return new Query(GetPartnerController().Table) {Relationship = GetRelationship()};
        }

        public Query Query(Func<T, Query> controllerAction)
        {
            var query = controllerAction(GetPartnerController().Instance);
            query.Relationship = GetRelationship();
            return query;
        }
    }
}