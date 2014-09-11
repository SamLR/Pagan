using Pagan.Conditions;
using Pagan.Relationships;

namespace Pagan.SqlObjects
{
    /// <summary>
    /// Defines a SqlTable participating in a join
    /// </summary>
    public class JoinedTable
    {
        public Table Table { get; set; }
        public Condition JoinCondition { get; set; }
        public Multiplicity Multiplicity { get; set; }
        public RelationshipEnd End { get; set; }
    }
}