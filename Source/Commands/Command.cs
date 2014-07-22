using System;
using System.Linq;
using Pagan.Queries;

namespace Pagan.Commands
{
    public class Command : ICommand
    {
        internal Command(Table table, CommandType commandType, CommandColumn[] columns)
        {
            if (table == null) throw new ArgumentNullException("table");

            Table = table;
            CommandType = commandType;

            switch (CommandType)
            {
                case CommandType.Insert:
                    if (columns == null || columns.Length == 0)
                        throw CommandError.EmptyColumnValues(commandType, table);

                    var requiredKeyCount = table.KeyColumns.Count(c => !c.DbGenerated);

                    var providedKeyCount = columns.Count(c => c.Column.IsKey);

                    if (providedKeyCount != requiredKeyCount)
                        throw CommandError.InvalidNumberOfKeyValues(table, requiredKeyCount, providedKeyCount);

                    Columns = columns;

                    AutoId = table.KeyColumns.Any(c => c.DbGenerated);

                    break;

                case CommandType.Update:
                    if (columns == null || columns.Length == 0)
                        throw CommandError.EmptyColumnValues(commandType, table);

                    if (columns.Any(c => c.Column.IsKey))
                        throw CommandError.AttemptToAlterKeyValues(table);

                    Columns = columns;
                    break;

                case CommandType.Delete:
                    Columns = Enumerable.Empty<CommandColumn>().ToArray();
                    break;
            }
        }

        public bool AutoId { get; private set; }

        public Table Table { get; private set; }
        
        public CommandType CommandType { get; private set; }

        public CommandColumn[] Columns { get; private set; }

        public FilterExpression Filter { get; private set; }


        public Query SelectQuery()
        {
            return Table.Where(Filter);
        }

        public Command Where(FilterExpression filter)
        {
            if (CommandType == CommandType.Insert)
                throw CommandError.FilterInsertError(Table);

            Filter = filter;
            return this;
        }
    }
}
