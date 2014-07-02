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

        // implemented by ChildRef and ParentRef
        protected abstract Relationship GetRelationship();

        public override Query Query()
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