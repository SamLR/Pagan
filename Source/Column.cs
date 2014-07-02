using System;
using Pagan.Commands;
using Pagan.Queries;

namespace Pagan
{
    public class Column : IEquatable<Column>
    {
        #region Equality

        public bool Equals(Column other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Table.Schema.DbName, other.Table.Schema.DbName) 
                && string.Equals(Table.Name, other.Table.Name)
                && string.Equals(Table.DbName, other.Table.DbName) 
                && string.Equals(Name, other.Name) 
                && string.Equals(DbName, other.DbName);
        }
        public override bool Equals(object obj)
        {
            var column = obj as Column;
            if (ReferenceEquals(null, column)) return false;
            return ReferenceEquals(this, column) || Equals(column);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Table.Schema.DbName.GetHashCode();
                hashCode = (hashCode * 397) ^ Table.Name.GetHashCode();
                hashCode = (hashCode * 397) ^ Table.DbName.GetHashCode();
                hashCode = (hashCode*397) ^ Name.GetHashCode();
                hashCode = (hashCode*397) ^ DbName.GetHashCode();
                return hashCode;
            }
        }

        #endregion

        public Column(Table table, string name)
        {
            Table = table;
            Name = name;
        }

        public Table Table { get; private set; }
        public string Name { get; private set; }
        public string DbName { get; set; }
        public bool IsKey { get; internal set; }

        public FilterExpression Like(string pattern)
        {
            return new FilterExpression(this, pattern, Operators.Like);
        }

        public SortingColumn Asc()
        {
            return new SortingColumn(this, SortDirection.Ascending);
        }

        public SortingColumn Desc()
        {
            return new SortingColumn(this, SortDirection.Descending);
        }

        public CommandColumn Is(object value)
        {
            return new CommandColumn(this, value);
        }

        #region operators

        public static FilterExpression operator ==(Column c, object value)
        {
            return value == null
                ? new FilterExpression(c, null, Operators.IsNull)
                : value.GetType().IsArray
                    ? new FilterExpression(c, value, Operators.In)
                    : new FilterExpression(c, value, Operators.Equal);
        }

        public static FilterExpression operator !=(Column c, object value)
        {
            return value == null
                ? new FilterExpression(c, null, Operators.IsNotNull)
                : value.GetType().IsArray
                    ? new FilterExpression(c, value, Operators.NotIn)
                    : new FilterExpression(c, value, Operators.NotEqual);
        }

        public static FilterExpression operator >=(Column c, object value)
        {
            return new FilterExpression(c, value, Operators.GreaterOrEqual);
        }

        public static FilterExpression operator <=(Column c, object value)
        {
            return new FilterExpression(c, value, Operators.LessOrEqual);
        }

        public static FilterExpression operator >(Column c, object value)
        {
            return new FilterExpression(c, value, Operators.GreaterThan);
        }

        public static FilterExpression operator <(Column c, object value)
        {
            return new FilterExpression(c, value, Operators.LessThan);
        }

        public static FilterExpression operator !(Column c)
        {
            return new FilterExpression(c, false, Operators.Equal);
        }

        #endregion
    }
}
