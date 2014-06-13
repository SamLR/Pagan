using Pagan.Registry;

namespace Pagan.DbComponents
{
    public class Column
    {
        public Column(Controller controller, string name)
        {
            Controller = controller;
            Name = name;
        }

        public Controller Controller { get; private set; }
        public string Name { get; private set; }
        public string DbName { get; set; }
        public Table Table { get { return Controller.Table; } }
        public int? KeyIndex { get; internal set; }
    }
}
