using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pagan.Queries;

namespace Pagan.Commands
{
    public class Command
    {
        private int _keyCount;

        public Command(Table table, object settings, CommandType commandType, FilterExpression filter=null)
        {
            Table = table;

            CommandType = commandType;

            Filter = filter;

            Columns = settings
                .GetType()
                .GetProperties()
                .Select(p => CreateCommandColumn(p, settings));

            if(_keyCount!=Table.KeyColumns.Length)
                throw new Exception("Not enough keys");
        }

        public Table Table { get; private set; }
        
        public CommandType CommandType { get; private set; }
        
        public IEnumerable<CommandColumn> Columns { get; private set; }

        public FilterExpression Filter { get; private set; }

        private CommandColumn CreateCommandColumn(PropertyInfo property, object settings)
        {
            Column column;
            
            if(!Table.Columns.TryGetColumn(property.Name, out column))
                throw new Exception("No such column");

            if (column.IsKey) _keyCount++;

            return new CommandColumn(column, property.GetValue(settings, null));
        }
    }
}
