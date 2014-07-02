using Pagan.Queries;

namespace Pagan.Relationships
{
    public interface ILinkRef
    {
        Table Table { get; }
        string Name { get; }
    }
}