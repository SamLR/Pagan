using Pagan.SqlObjects;

namespace Pagan.Conditions
{
    public class FieldValueCondition : Condition
    {
        internal FieldValueCondition(Field left, object right, ComparisonOperator op)
        {
            Left = left;
            Right = right;
            Operator = op;
        }

        public ComparisonOperator Operator { get; private set; }
        public Field Left { get; private set; }
        public object Right { get; private set; }
    }
}