using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Pagan.Queries
{
    public class QueryResult
    {
        public QueryResult(string name, IEnumerable<QueryColumn> columns, IEnumerable<Query> children)
        {
            Name = name;
            Columns = columns;
            Children = children;
        }

        protected readonly IEnumerable<QueryColumn> Columns;
        protected readonly IEnumerable<Query> Children;
        protected QueryEntity Current;
        protected bool Changed;

        public string Name { get; private set; }

        public virtual bool IsNull { get { return ReferenceEquals(null, Current); } }
        
        public void Read(IDataRecord data)
        {
            var entity = CreateEntity();
            entity.ReadKeys(data);

            if (entity.IsNull)
            {
                OnNullEntity(entity);
                return;
            }

            if (entity.Equals(Current))
            {
                OnRepeatEntity(entity);
                entity.ReadChildren(data);
                return;
            }

            OnNewEntity(entity, data);
            entity.ReadChildren(data);

        }

        protected virtual void OnNullEntity(QueryEntity entity)
        {
            Changed = !ReferenceEquals(Current, null);
            Current = null;
        }

        protected virtual void OnRepeatEntity(QueryEntity entity)
        {
            Changed = false;
        }

        protected virtual void OnNewEntity(QueryEntity entity, IDataRecord data)
        {
            entity.ReadFields(data);
            Changed = true;
            Current = entity;
        }

        private QueryEntity CreateEntity()
        {
            return new QueryEntity(
                Columns.Where(c => c.IsKey),
                Columns.Where(c => !c.IsKey),
                Children.Select(x => x.CreateQueryResult())
                );
        }

        public virtual object GetValue()
        {
            return ReferenceEquals(null, Current) ? null : Current.GetValue();
        }

        public IEnumerable<dynamic> Spool(IDataReader data)
        {
            while (data.Read())
            {
                var previous = Current;

                Read(data);

                if (Changed && !ReferenceEquals(null, previous))
                    yield return previous;
            }
            yield return Current;
        }
    }
}