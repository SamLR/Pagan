namespace Pagan.SqlObjects
{
    /// <summary>
    /// Defines a SqlField participating in the Ordering
    /// </summary>
    class OrderedField
    {
        public SortDirection Direction { get; set; }
        public SqlField Field { get; set; }
    }
}