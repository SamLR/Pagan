namespace Pagan.Conditions
{
    public partial class LogicalGroup
    {
        public static LogicalGroup operator &(LogicalGroup left, LogicalGroup right)
        {
            if (left.Operator == LogicalOperator.And && right.Operator == LogicalOperator.And)
            {
                foreach (var condition in right.Conditions)
                    left.Conditions.Add(condition);
                return left;
            }

            if (left.Operator == LogicalOperator.And && right.Operator == LogicalOperator.Or)
            {
                left.Conditions.Add(right);
                return left;
            }

            if (left.Operator == LogicalOperator.Or && right.Operator == LogicalOperator.And)
            {
                right.Conditions.Add(left);
                return right;
            }

            return And(left, right);
        }

        public static LogicalGroup operator |(LogicalGroup left, LogicalGroup right)
        {
            if (left.Operator == LogicalOperator.Or && right.Operator == LogicalOperator.Or)
            {
                foreach (var condition in right.Conditions)
                    left.Conditions.Add(condition);
                return left;
            }

            if (left.Operator == LogicalOperator.Or && right.Operator == LogicalOperator.And)
            {
                left.Conditions.Add(right);
                return left;
            }

            if (left.Operator == LogicalOperator.And && right.Operator == LogicalOperator.Or)
            {
                right.Conditions.Add(left);
                return right;
            }

            return Or(left, right);
        }
    }
}
