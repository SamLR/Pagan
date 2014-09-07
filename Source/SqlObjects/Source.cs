using System.Collections.Generic;

namespace Pagan.SqlObjects
{
    /// <summary>
    /// Defines the source of a query. 
    /// Can be a single SqlTable, a main SqlTable and joins, or a subquery
    /// </summary>
    class Source
    {
        public SqlTable Table { get; set; }
        public List<JoinedTable> JoinedTables { get; set; }
        public SqlQuery SubSqlQuery { get; set; }
    }
}
