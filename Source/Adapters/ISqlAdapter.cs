using Pagan.SqlObjects;

namespace Pagan.Adapters
{
    /// <summary>
    /// Defines an adapter that abstracts provider specific bits
    /// </summary>
    interface ISqlAdapter
    {
        string Table(Table table);
        string Field(Field field);
        string ParameterPrefix { get; }
    }
}