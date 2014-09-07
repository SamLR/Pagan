namespace Pagan.SqlObjects
{
    /// <summary>
    /// Defines a SqlTable participating in a join
    /// </summary>
    class JoinedTable
    {
        public SqlTable Table { get; set; }
        public JoinType JoinType { get; set; }
    }
}