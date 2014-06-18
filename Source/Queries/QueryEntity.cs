using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace Pagan.Queries
{
    public class QueryEntity : IEquatable<QueryEntity>
    {
        #region Equality
        
        public bool Equals(QueryEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || _keys.Equals(other._keys);
        }

        public override bool Equals(object obj)
        {
            var en = obj as QueryEntity;
            if (ReferenceEquals(null, en)) return false;
            return ReferenceEquals(this, en) || Equals(en);
        }
        public override int GetHashCode()
        {
            return _keys.GetHashCode();
        }

        #endregion

        private readonly List<QueryColumn> _keys;
        private readonly List<QueryColumn> _fields;
        private readonly List<QueryResult> _children;

        public QueryEntity(IEnumerable<QueryColumn> keys, IEnumerable<QueryColumn> fields, IEnumerable<QueryResult> children)
        {
            _keys = keys == null ? new List<QueryColumn>() : new List<QueryColumn>(keys);
            _fields = fields == null ? new List<QueryColumn>() : new List<QueryColumn>(fields);
            _children = children == null ? new List<QueryResult>() : new List<QueryResult>(children);
        }

        public bool IsNull { get { return _keys.Any(k => k.IsNull); } }

        public void ReadKeys(IDataRecord data)
        {
            _keys.ForEach(f => f.Read(data));
        }

        public void ReadFields(IDataRecord data)
        {
            _fields.ForEach(f => f.Read(data));
        }

        public void ReadChildren(IDataRecord data)
        {
            _children.ForEach(q=> q.Read(data));
        }

        public object GetValue()
        {
            IDictionary<string, object> record = new ExpandoObject();
            _keys.Union(_fields).Where(k => !k.IsNull).ForEach(x => record[x.Name] = x.Value);
            _children.Where(k => !k.IsNull).ForEach(x => record[x.Name] = x.GetValue());
            return record;
        }
    }

}