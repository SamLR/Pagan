using System;

namespace Pagan.Queries
{
    public class FilterExpression
    {
        internal FilterExpression(object left, object right, Operators op)
        {
            Left = left as FilterExpression;
            Right = right as FilterExpression;
            LeftColumn = left as Column;
            RightColumn = right as Column;
            Value = Right == null && ReferenceEquals(RightColumn, null) ? right : null;
            Operator = op;

            if (AndOr && (Left == null || Right == null)) // should never happen but just in case
                throw new Exception("And/Or expressions only act on two other FilterExpressions");
        }

        internal bool AndOr
        {
            get
            {
                return Operator == Operators.And || Operator == Operators.Or;
            }
        }

        public FilterExpression Left { get; private set; }
        public FilterExpression Right { get; private set; }
        public Column LeftColumn { get; private set; }
        public Column RightColumn { get; private set; }
        public object Value { get; private set; }
        public Operators Operator { get; internal set; }

        internal void Negate()
        {
            Operator = (Operators)(-((int)Operator));
            if (AndOr)
            {
                Left.Negate();
                Right.Negate();
            }
        }

        #region operators

        public static FilterExpression operator !(FilterExpression f)
        {
            f.Negate();
            return f;
        }

        public static implicit operator FilterExpression(Column c)
        {
            return new FilterExpression(c, true, Operators.Equal);
        }

        public static FilterExpression operator &(FilterExpression left, FilterExpression right)
        {
            return new FilterExpression(left, right, Operators.And);
        }

        public static FilterExpression operator |(FilterExpression left, FilterExpression right)
        {
            return new FilterExpression(left, right, Operators.Or);
        }

        public static bool operator true(FilterExpression e)
        {
            return false;
        }

        public static bool operator false(FilterExpression e)
        {
            return false;
        }

        #endregion
    }
}