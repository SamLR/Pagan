using System;
using System.Collections.Generic;

namespace Pagan.Registry
{
    public class TableFactory: ITableFactory
    {
        private readonly Dictionary<Type, Table> _controllers;
        private readonly ITableConventions _conventions;

        public TableFactory(ITableConventions conventions)
        {
            _controllers = new Dictionary<Type, Table>();
            _conventions = conventions;
        }

        public Table<T> GetTable<T>()
        {
            Table table;
            if (!_controllers.TryGetValue(typeof (T), out table))
                _controllers[typeof (T)] = table = new Table<T>(this, _conventions);

            return (Table<T>) table;
        }
    }
}