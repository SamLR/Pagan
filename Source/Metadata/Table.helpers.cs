using System;
using System.Collections.Generic;
using System.Linq;

namespace Pagan.Metadata
{
    static class TableHelpers
    {
        public static Field GetField(this Table t, string name, bool strict = true)
        {
            Func<Field, bool> match =
                f => String.Equals(f.PropertyInfo.Name, name, StringComparison.InvariantCultureIgnoreCase);

            return
                strict
                    ? t.Fields.First(match)
                    : t.Fields.FirstOrDefault(match);
        }

        public static IEnumerable<Field> GetAllFields(this Table t, IEnumerable<string> names)
        {
            return names.Select(n => t.GetField(n));
        }

        public static IEnumerable<Field> GetAnyFields(this Table t, IEnumerable<string> names)
        {
            return names.Select(n => t.GetField(n, false)).Where(f => f != null);
        }

        public static Field SelectField(this Table t, IEnumerable<string> names)
        {
            return t.GetAnyFields(names).FirstOrDefault();
        }
    }
}