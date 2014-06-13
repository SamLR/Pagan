using System;
using System.Collections.Generic;
using Pagan.Registry;
using Pagan.Relationships;

namespace Pagan
{
    public static class ObjectExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T,int> action)
        {
            var count = 0;
            foreach (var item in items)
                action(item, count++);
        }

        public static bool HasForeignKey(this IDependent dependent)
        {
            return dependent.ForeignKeyColumns != null && dependent.ForeignKeyColumns.Length > 0;
        }


        public static bool HasPrimaryKey(this Table table)
        {
            return table.KeyColumns != null && table.KeyColumns.Length > 0;
        }

    }

    
}
