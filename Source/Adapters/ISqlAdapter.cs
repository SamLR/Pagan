using Pagan.SqlObjects;

namespace Pagan.Adapters
{
    /// <summary>
    /// Defines an adapter that abstracts provider specific bits
    /// </summary>
    interface ISqlAdapter
    {
        string Table(SqlTable table);
        string Field(SqlField field);
        string ParameterPrefix { get; }
    }
}