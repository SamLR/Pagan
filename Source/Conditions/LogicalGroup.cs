using System.Collections.Generic;

namespace Pagan.Conditions
{
    public partial class LogicalGroup: Condition
    {
        internal static LogicalGroup And(IEnumerable<Condition> conditions)
        {
            return new LogicalGroup(LogicalOperator.And, conditions);
        }

        internal static LogicalGroup Or(IEnumerable<Condition> conditions)
        {
            return new LogicalGroup(LogicalOperator.Or, conditions);
        }

        internal static LogicalGroup And(params Condition[] conditions)
        {
            return new LogicalGroup(LogicalOperator.And, conditions);
        }

        internal static LogicalGroup Or(params Condition[] conditions)
        {
            return new LogicalGroup(LogicalOperator.Or, conditions);
        }

        internal LogicalGroup(LogicalOperator op, IEnumerable<Condition> conditions=null)
        {
            Operator = op;
            Conditions = conditions == null ? new List<Condition>() : new List<Condition>(conditions);
        }

        public LogicalOperator Operator { get; private set; }

        public List<Condition> Conditions { get; private set; }

        public LogicalGroup And(Condition condition)
        {
            if (Operator != LogicalOperator.And) return And(this, condition);
            Conditions.Add(condition);
            return this;
        }

        public LogicalGroup Or(Condition condition)
        {
            if (Operator != LogicalOperator.Or) return Or(this, condition);
            Conditions.Add(condition);
            return this;
        }

    }
}