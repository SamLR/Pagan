using Pagan.Conditions;

namespace Pagan.SqlObjects
{
    public partial class Field
    {
        public static FieldComparison operator ==(Field left, object right)
        {
            return new FieldComparison(left, right, ComparisonOperator.Equal);
        }

        public static FieldComparison operator !=(Field left, object right)
        {
            return new FieldComparison(left, right, ComparisonOperator.NotEqual);
        }

        public static FieldComparison operator >=(Field left, object right)
        {
            return new FieldComparison(left, right, ComparisonOperator.GreaterOrEqual);
        }

        public static FieldComparison operator <=(Field left, object right)
        {
            return new FieldComparison(left, right, ComparisonOperator.LessOrEqual);
        }
        
        public static FieldComparison operator >(Field left, object right)
        {
            return new FieldComparison(left, right, ComparisonOperator.GreaterThan);
        }
        
        public static FieldComparison operator <(Field left, object right)
        {
            return new FieldComparison(left, right, ComparisonOperator.LessThan);
        }
        
        public static FieldComparison operator !(Field field)
        {
            return field == false;
        }
        
        public static implicit operator FieldComparison(Field field)
        {
            return field == true;
        }
    }
}
