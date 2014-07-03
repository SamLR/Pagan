using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace Pagan.Results
{
    class ReaderConverter
    {
        public static IEnumerable<dynamic> ToDynamic(IDataReader data)
        {
            var names = new string[data.FieldCount];
            for (var i = 0; i < data.FieldCount; i++)
            {
                names[i] = data.GetName(i);
            }

            while (data.Read())
            {
                IDictionary<string, object> result = new ExpandoObject();
                names.ForEach((n, i) => result[names[i]] = data.GetValue(i));
                yield return result;
            }
        } 
    }
}
