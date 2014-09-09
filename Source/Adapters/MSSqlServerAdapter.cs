using Pagan.SqlObjects;

namespace Pagan.Adapters
{
    class MSSqlServerAdapter : ISqlAdapter
    {
        public string Table(Table table)
        {
            return table.Schema == null
                ? string.Format("[{0}]", table.Name)
                : string.Format("[{0}].[{1}]", table.Schema.Name, table.Name);
        }

        public string Field(Field field)
        {
            return string.Format("{0}.[{1}]", Table(field.Table), field.Name);
        }

        public string ParameterPrefix { get { return "@"; } }
    }
}