using System;
using System.Collections.Generic;
using Pagan.Results;

namespace Pagan.Queries
{
    public class QueryColumn : IEquatable<QueryColumn>
    {
        #region Equality

        public bool Equals(QueryColumn other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) 
                || EqualityComparer<Column>.Default.Equals(Column, other.Column);
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
                return hashCode;
            }
        }

        #endregion

        public static implicit operator QueryColumn(Column c)
        {
            return new QueryColumn(c);
        }

        public QueryColumn(Column column)
        {
            Column = column;
        }

        public Column Column { get; private set; }
        public int Position { get; internal set; }

        public EntityField CreateField()
        {
            return new EntityField(Column.Name, Position);
        }
    }
}