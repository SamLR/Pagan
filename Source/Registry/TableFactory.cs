using System;
using System.Collections.Generic;

namespace Pagan.Registry
{
    public class TableFactory: ITableFactory
    {
        private readonly Dictionary<Type, Table> _controllers;
        private readonly ITableConfiguration _tableConfig;

        public TableFactory(ITableConfiguration tableConfig)
        {
            _controllers = new Dictionary<Type, Table>();
            _tableConfig = tableConfig;
        }

        public Table<T> GetTable<T>()
        {
            Table table;
            if (!_controllers.TryGetValue(typeof (T), out table))
                _controllers[typeof (T)] = table = new Table<T>(this, _tableConfig);

            return (Table<T>) table;
        }
    }
}