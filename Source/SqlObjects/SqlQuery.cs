using System.Collections.Generic;
using Pagan.Conditions;

namespace Pagan.SqlObjects
{
    /// <summary>
    /// Defines a standard SQL query
    /// </summary>
    class SqlQuery
    {
        public SqlQuery(Table table)
        {
            Source = new Source {Table = table};
            Selection = new List<SelectedField>();
            Ordering = new List<OrderedField>();
            Grouping = new List<Field>();
        }

        public Source Source { get; private set; }
        public List<SelectedField> Selection { get; private set; }
        public Condition Condition { get; private set; }
        public List<OrderedField> Ordering { get; private set; }
        public List<Field> Grouping { get; private set; }
        public Condition Having { get; private set; }
    }
}