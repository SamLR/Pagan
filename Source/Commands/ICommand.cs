using System.Collections.Generic;
using Pagan.Queries;

namespace Pagan.Commands
{
    public interface ICommand
    {
        Table Table { get; }
        CommandType CommandType { get; }
        IEnumerable<CommandColumn> Columns { get; }
        FilterExpression Filter { get; }
    }
}