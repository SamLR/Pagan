using Pagan.Relationships;

namespace Pagan.Registry
{
    public interface ITableConventions
    {
        string GetSchemaDbName(Table table);
        string GetTableDbName(Table table);
        string GetColumnDbName(Column column);
        Column[] GetPrimaryKey(Table table);
        void SetDefaultForeignKey(IDependent dependent, Column[] columns);
    }
}