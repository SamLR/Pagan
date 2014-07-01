namespace Pagan.Commands
{
    public class CommandColumn
    {
        public CommandColumn(Column column, object value)
        {
            Value = value;
            Column = column;
        }

        public Column Column { get; private set; }
        public object Value { get; private set; }
    }
}