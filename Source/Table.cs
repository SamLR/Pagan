using System;
using Pagan.Queries;
using Pagan.Registry;
using Pagan.Relationships;

namespace Pagan
{
    public abstract class Table: IQueryBuilder
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
            KeyColumns.ForEach(c=> c.IsKey = true);
        }
        
        internal ITableFactory Factory;
        internal ITableConventions Conventions;


        #region Implementation of IQueryBuilder

        public Query Select(params QueryColumn[] selected)
        {
            return ((Query) this).Select(selected);
        }

        public Query OrderBy(params SortingColumn[] sorting)
        {
            return ((Query) this).OrderBy(sorting);
        }

        public Query Where(FilterExpression filter)
        {
            return ((Query) this).Where(filter);
        }
        
        #endregion
    }
}