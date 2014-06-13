using System;
using Pagan.Queries;

namespace Pagan.Relationships
{
    public abstract class LinkRef<T>: LinkRef
    {
        protected LinkRef(Table table, string name): base(table, name)
        {
            PartnerControllerType = typeof (T);
        }

        private Table<T> GetPartnerTable()
        {
            return Table.Factory.GetTable<T>();
        }

        protected LinkRef GetPartnerRef()
        {
            return GetPartnerTable().GetLinkRef(Table.ControllerType);
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
            return new Query(GetPartnerTable()) {Relationship = GetRelationship()};
        }

        public Query Query(Func<T, Query> controllerAction)
        {
            var query = controllerAction(GetPartnerTable().Controller);
            query.Relationship = GetRelationship();
            return query;
        }
    }
}