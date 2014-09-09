using System.Collections.Generic;

namespace Pagan.Conditions
{
    public class LogicalGroup : Condition
    {
        private readonly List<Condition> _conditions;

        internal static Condition And(IEnumerable<Condition> conditions)
        {
            return new LogicalGroup(LogicalOperator.And, conditions);
        }

        internal static Condition Or(IEnumerable<Condition> conditions)
        {
            return new LogicalGroup(LogicalOperator.Or, conditions);
        }

        internal LogicalGroup(LogicalOperator op, IEnumerable<Condition> conditions=null)
        {
            Operator = op;
            _conditions = conditions == null ? new List<Condition>() : new List<Condition>(conditions);
        }

        public LogicalOperator Operator { get; private set; }

        public IEnumerable<Condition> Conditions
        {
            get { return _conditions; }
        }

        internal void Add(Condition condition)
        {
            _conditions.Add(condition);
        }
    }
}