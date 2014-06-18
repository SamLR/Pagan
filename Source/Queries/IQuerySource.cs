using Pagan.Relationships;

namespace Pagan.Queries
{
    public interface IQuerySource
    {
        string Name { get; }
        Table Table { get; }
        Relationship Relationship { get; }
    }
}