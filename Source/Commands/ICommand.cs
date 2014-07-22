using Pagan.Queries;

namespace Pagan.Commands
{
    public interface ICommand
    {
        Table Table { get; }
        CommandColumn[] Columns { get; }
        FilterExpression Filter { get; }
        CommandType CommandType { get; }
        bool AutoId { get; }
    }
}