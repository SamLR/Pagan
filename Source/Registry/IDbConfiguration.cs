using System.Collections.Generic;
using Pagan.DbComponents;
using Pagan.Relationships;

namespace Pagan.Registry
{
    public interface IDbConfiguration
    {
        string GetDefaultSchemaName();
        void SetDefaultColumnDbName(Column column);
        void SetDefaultPrimaryKey(Column[] columns);
        void SetDefaultForeignKey(IDependent dependent, Column[] columns);
    }
}