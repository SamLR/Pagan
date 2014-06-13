using Pagan.Registry;

namespace Pagan.DbComponents
{
    public class Table
    {
        public Table(Controller controller, string name)
        {
            Controller = controller;
            DbName = name;
        }

        public Controller Controller { get; private set; }
        public string DbName { get; private set; }
        public Schema Schema { get { return Controller.Schema; } }

        public void SetKeys(params Column[] keyColumns)
        {
            keyColumns.ForEach((c, i) =>
            {
                c.KeyIndex = i;
                Controller.KeyColumns = keyColumns;
            });
        }
    }
}