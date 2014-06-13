using Pagan.Registry;

namespace Pagan
{
    public class Schema
    {
        public Schema(Table table, string dbName)
        {
            Table = table;
            DbName = dbName;
        }

        public string DbName { get; private set; }
        public Table Table { get; private set; }
    }
}