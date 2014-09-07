using System.Collections.Generic;

namespace Pagan.SqlObjects
{
    /// <summary>
    /// Defines a standard SQL query
    /// </summary>
    class SqlQuery
    {
        public SqlQuery(SqlTable table)
        {
            Source = new Source {Table = table};
            Selection = new List<SelectedField>();
            Condition = new Filter();
            Ordering = new List<OrderedField>();
            Grouping = new List<SqlField>();
            Having = new Filter();
        }

        public Source Source { get; private set; }
        public List<SelectedField> Selection { get; private set; }
        public Filter Condition { get; private set; }
        public List<OrderedField> Ordering { get; private set; }
        public List<SqlField> Grouping { get; private set; }
        public Filter Having { get; private set; }
    }
}