using System;
using System.Collections.Generic;
using System.Linq;
using Pagan.Queries;

namespace Pagan.Commands
{
    public class Command : ICommand
    {
        public Command(Table table, CommandColumn[] columns, CommandType commandType)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (columns == null) throw new ArgumentNullException("columns");

            Table = table;

            CommandType = commandType;

            if (columns.Count(x => x.Column.IsKey) != Table.KeyColumns.Length)
                throw new Exception("Not enough keys");
            
            Columns = CalculateColumns(columns);
            Filter = CalculateFilter(columns);
        }

        public Table Table { get; private set; }
        
        public CommandType CommandType { get; private set; }

        public IEnumerable<CommandColumn> Columns { get; private set; }

        public FilterExpression Filter { get; private set; }

        private IEnumerable<CommandColumn> CalculateColumns(IEnumerable<CommandColumn> columns)
        {
            switch (CommandType)
            {
                case CommandType.Delete:
                case CommandType.Update:
                    return columns.Where(c => !c.Column.IsKey);

                default:
                    return columns;
            }
        }

        private FilterExpression CalculateFilter(IEnumerable<CommandColumn> columns)
        {
            switch (CommandType)
            {
                case CommandType.Delete:
                case CommandType.Update:
                    return columns.Where(c=> c.Column.IsKey).Select(c => c.Column == c.Value).All();

                default:
                    return null;
            }
        }

        public Query SelectQuery()
        {
            return Table.Where(Filter);
        }
    }
}
