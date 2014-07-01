using System.Collections.Generic;
using System.Data;
using System.Linq;
using Pagan.Queries;

namespace Pagan.Results
{
    public class EntityMultiSet : EntitySet
    {
        private readonly List<Entity> _entities;

        public EntityMultiSet(string name, IEnumerable<QueryColumn> columns, IEnumerable<Query> children)
            : base(name, columns, children)
        {
            _entities = new List<Entity>();
        }

        protected override void OnNewEntity(ref Entity entity, IDataRecord data)
        {
            base.OnNewEntity(ref entity, data);

            var existing = _entities.Find(entity.Equals);

            if (existing != null)
            {
                entity = existing;
                return;
            }

            _entities.Add(entity);
        }

        public override bool IsNull
        {
            get { return _entities.Count == 0; }
        }


        public override object GetValue()
        {
            return _entities.Count == 0
                ? null
                : _entities.Select(n => n.GetValue()).ToArray();
        }
    }

    
}