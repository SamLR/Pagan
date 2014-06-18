using System;
using System.Collections.Generic;
using System.Data;

namespace Pagan.Queries
{
    public class QueryColumn : IEquatable<QueryColumn>
    {
        #region Equality

        public bool Equals(QueryColumn other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<Column>.Default.Equals(Column, other.Column) 
                && Equals(Value, other.Value);
        }
        public override bool Equals(object obj)
        {
            var field = obj as QueryColumn;
            if (ReferenceEquals(null, field)) return false;
            return ReferenceEquals(this, field) || Equals(field);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Column != null ? Column.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Position.GetHashCode();
                hashCode = (hashCode*397) ^ IsKey.GetHashCode();
                hashCode = (hashCode*397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion

        public QueryColumn(Column column, bool isKey)
        {
            Column = column;
            IsKey = isKey;
        }

        public Column Column { get; private set; }
        public bool IsKey { get; private set; }
        public int Position { get; internal set; }

        #region Implementation of IDataNode

        public string Name { get { return Column.Name; } }

        public bool IsNull { get { return ReferenceEquals(null, Value); } }

        public void Read(IDataRecord data)
        {
            Value = data.IsDBNull(Position)
                ? null
                : data.GetValue(Position);
        }

        public object Value { get; private set; }

        #endregion
    }
}