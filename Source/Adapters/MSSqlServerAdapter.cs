using Pagan.SqlObjects;

namespace Pagan.Adapters
{
    class MSSqlServerAdapter : ISqlAdapter
    {
        public string Table(SqlTable table)
        {
            return table.Schema == null
                ? string.Format("[{0}]", table.Name)
                : string.Format("[{0}].[{1}]", table.Schema, table.Name);
        }

        public string Field(SqlField field)
        {
            return string.Format("{0}.[{1}]", Table(field.Table), field.Name);
        }

        public string ParameterPrefix { get { return "@"; } }
    }
}