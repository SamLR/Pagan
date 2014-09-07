using System;

namespace Pagan.Metadata
{
    interface ITableFactory
    {
        Table GetTable(Type entityType);
        Table GetTable<T>();
    }
}