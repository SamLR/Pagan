namespace Pagan
{
    public class Column
    {
        public Column(Table table, string name)
        {
            Table = table;
            Name = name;
        }

        public Table Table { get; private set; }
        public string Name { get; private set; }
        public string DbName { get; set; }
    }
}
