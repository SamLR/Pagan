using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace Pagan.Results
{
    public class Entity : IEquatable<Entity>
    {
        #region Equality
        
        public bool Equals(Entity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (_keys.Count != other._keys.Count) return false;
            return _keys.Select((k, i) => k.Equals(other._keys[i])).All(x => x);
        }

        public override bool Equals(object obj)
        {
            var en = obj as Entity;
            if (ReferenceEquals(null, en)) return false;
            return ReferenceEquals(this, en) || Equals(en);
        }
        public override int GetHashCode()
        {
            return _keys.GetHashCode();
        }

        #endregion

        private readonly List<EntityField> _keys;
        private readonly List<EntityField> _fields;
        private readonly List<EntitySet> _children;

        public Entity(IEnumerable<EntityField> keys, IEnumerable<EntityField> fields, IEnumerable<EntitySet> children)
        {
            _keys = keys == null ? new List<EntityField>() : new List<EntityField>(keys);
            _fields = fields == null ? new List<EntityField>() : new List<EntityField>(fields);
            _children = children == null ? new List<EntitySet>() : new List<EntitySet>(children);
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