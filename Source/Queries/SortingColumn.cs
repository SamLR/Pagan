namespace Pagan.Queries
{
    public class SortingColumn
    {
        public static implicit operator SortingColumn(Column c)
        {
            return c.Asc();
        }

        public SortingColumn(Column column, SortDirection direction)
        {
            Column = column;
            Direction = direction;
        }

        public Column Column { get; private set; }
        public SortDirection Direction { get; private set; }
    }
}
