namespace Pagan.SqlObjects
{
    /// <summary>
    /// Defines an aggregate function being applied to a selected SqlField
    /// </summary>
    class AggregatedField : SelectedField
    {
        public AggregateFunction Aggregation { get; set; }
    }
}