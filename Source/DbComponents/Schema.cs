using Pagan.Registry;

namespace Pagan.DbComponents
{
    public class Schema
    {
        public Schema(Controller controller, string dbName)
        {
            Controller = controller;
            DbName = dbName;
        }

        public string DbName { get; private set; }
        public Controller Controller { get; private set; }
    }
}