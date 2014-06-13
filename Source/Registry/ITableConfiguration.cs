using Pagan.Relationships;

namespace Pagan.Registry
{
    public interface ITableConfiguration
    {
        string GetDefaultSchemaName();
        void SetDefaultColumnDbName(Column column);
        void SetDefaultPrimaryKey(Table table);
        void SetDefaultForeignKey(IDependent dependent, Column[] columns);
    }
}