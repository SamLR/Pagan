using Pagan.Conditions;
using Pagan.Configuration;
using Pagan.Relationships;

namespace Pagan.SqlObjects
{
    /// <summary>
    /// Defines a SqlTable participating in a join
    /// </summary>
    public class JoinedTable
    {
        internal IDefinition Definition;
        public Table Table { get { return Definition.Table; } }
        public Condition JoinCondition { get; set; }
        public Multiplicity Multiplicity { get; set; }
        public RelationshipEnd End { get; set; }
    }
}