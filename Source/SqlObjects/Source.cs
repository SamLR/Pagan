using System.Collections.Generic;
using Pagan.Configuration;

namespace Pagan.SqlObjects
{
    /// <summary>
    /// Defines the source of a query. 
    /// Can be a single SqlTable, a main SqlTable and joins, or a subquery
    /// </summary>
    class Source
    {
        public Source()
        {
            JoinedTables = new List<JoinedTable>();
        }

        internal IDefinition Definition;
        public Table Table { get { return Definition.Table; } }
        public List<JoinedTable> JoinedTables { get; private set; }
        public SqlQuery SubSqlQuery { get; set; }
    }
}
