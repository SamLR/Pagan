using System;

namespace Pagan.Conditions
{
    public abstract class Condition
    {
        public static Condition operator &(Condition left, Condition right)
        {
            var leftGroup = left as LogicalGroup;
            var rightGroup = right as LogicalGroup;

            if (leftGroup != null && rightGroup != null)
                return leftGroup && rightGroup;

            if (leftGroup == null && rightGroup == null)
                return LogicalGroup.And(left, right);

            return leftGroup != null ? leftGroup.And(right) : rightGroup.And(left);
        }

        public static Condition operator |(Condition left, Condition right)
        {
            var leftGroup = left as LogicalGroup;
            var rightGroup = right as LogicalGroup;

            if (leftGroup != null && rightGroup != null)
                return leftGroup || rightGroup;

            if (leftGroup == null && rightGroup == null)
                return LogicalGroup.Or(left, right);

            return leftGroup != null ? leftGroup.Or(right) : rightGroup.Or(left);
        }


        public static bool operator true(Condition c)
        {
            return false;
        }

        public static bool operator false(Condition c)
        {
            return false;
        }

        public static Condition operator !(Condition c)
        {
            c.Not = true;
            return c;
        }

        public bool Not { get; private set; }
    }
}
