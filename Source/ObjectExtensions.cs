using System;
using System.Collections.Generic;
using System.Linq;
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

        public static bool TryGetColumn(this Column[] columns, string match, out Column column)
        {
            column = columns
                .FirstOrDefault(c => String.Equals(c.DbName, match, StringComparison.InvariantCultureIgnoreCase));

            return !ReferenceEquals(null, column);
        }

        public static bool TryGetColumn(this Column[] columns, IEnumerable<string> matches, out Column column)
        {
            foreach (var match in matches)
                if (TryGetColumn(columns, match, out column)) return true;

            column = null;
            return false;
        }

        public static bool TryGetColumn(this Table table, string match, out Column column)
        {
            return table.Columns.TryGetColumn(match, out column);
        }

        public static bool TryGetColumn(this Table table, IEnumerable<string> matches, out Column column)
        {
            return table.Columns.TryGetColumn(matches, out column);
        }


        public static LinkRef GetLinkRef(this Table table, Type type)
        {
            return table.LinkRefs.First(r => r.PartnerControllerType == type);
        }

    }

    
}
