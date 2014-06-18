using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Pagan.Queries
{
    public class QueryMultiResult : QueryResult
    {
        private readonly List<QueryEntity> _entities;

        public QueryMultiResult(string name, IEnumerable<QueryColumn> columns, IEnumerable<Query> children)
            : base(name, columns, children)
        {
            _entities = new List<QueryEntity>();
        }

        protected override void OnNewEntity(QueryEntity entity, IDataRecord data)
        {
            base.OnNewEntity(entity, data);

            var existing = _entities.Find(entity.Equals);

            if (!ReferenceEquals(existing, null)) return;

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
                : _entities.Select(n => n.GetValue());
        }
    }

    
}