using Pagan.SqlObjects;

namespace Pagan.Conditions
{
    public class FieldMatchCondition : Condition
    {
        internal FieldMatchCondition(Field left, Field right)
        {
            Left = left;
            Right = right;
        }

        public Field Left { get; private set; }
        public Field Right { get; private set; }
    }
}