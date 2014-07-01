using System.Collections.Generic;
using System.Data;
using System.Linq;
using Pagan.Queries;

namespace Pagan.Results
{
    public class EntitySet
    {
        public EntitySet(string name, IEnumerable<QueryColumn> columns, IEnumerable<Query> children)
        {
            Name = name;
            Columns = columns;
            Children = children;
        }

        protected readonly IEnumerable<QueryColumn> Columns;
        protected readonly IEnumerable<Query> Children;
        protected Entity Current;
        protected bool Changed;

        public string Name { get; private set; }

        public virtual bool IsNull{ get { return Current == null; } }

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
                Current.ReadChildren(data);
                return;
            }

            OnNewEntity(ref entity, data);
            entity.ReadChildren(data);

        }

        protected virtual void OnNullEntity(Entity entity)
        {
            Changed = Current != null;
            Current = null;
        }

        protected virtual void OnRepeatEntity(Entity entity)
        {
            Changed = false;
        }

        protected virtual void OnNewEntity(ref Entity entity, IDataRecord data)
        {
            entity.ReadFields(data);
            Changed = true;
            Current = entity;
        }

        private Entity CreateEntity()
        {
            return new Entity(
                Columns.Where(c => c.Column.IsKey).Select(c=> c.CreateField()),
                Columns.Where(c => !c.Column.IsKey).Select(c => c.CreateField()),
                Children.Select(x => x.CreateEntitySet())
                );
        }

        public virtual object GetValue()
        {
            return Current == null ? null : Current.GetValue();
        }

        public IEnumerable<dynamic> Spool(IDataReader data)
        {
            while (data.Read())
            {
                var previous = Current;

                Read(data);

                if (Changed && !ReferenceEquals(null, previous))
                    yield return previous.GetValue();
            }

            yield return Current.GetValue();
        }
    }
}