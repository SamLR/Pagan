using System.Data;

namespace Pagan.Results
{
    public class EntityField
    {
        #region Equality

        public bool Equals(EntityField other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name.Equals(other.Name)
                   && Equals(Value, other.Value);
        }
        public override bool Equals(object obj)
        {
            var field = obj as EntityField;
            if (ReferenceEquals(null, field)) return false;
            return ReferenceEquals(this, field) || Equals(field);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion

        private readonly int _position;
        public EntityField(string name, int position)
        {
            Name = name;
            _position = position;
        }

        public string Name { get; private set; }
        public object Value { get; private set; }
        public bool IsNull { get { return Value == null; } }

        public void Read(IDataRecord data)
        {
            Value = data.IsDBNull(_position)
                ? null
                : data.GetValue(_position);
        }

    }
}