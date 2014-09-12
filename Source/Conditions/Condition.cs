namespace Pagan.Conditions
{
    public abstract class Condition
    {
        public static implicit operator bool(Condition c)
        {
            return true;
        }

        public static Condition operator &(Condition left, Condition right)
        {
            if (left == null) return right; // convenience to support c1 &= c2 = c2 when c1 is null

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
            if (left == null) return right; // convenience to support c1 &= c2 = c2 when c1 is null

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
