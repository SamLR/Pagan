using System.Reflection;
using Pagan.SqlObjects;

namespace Pagan.Metadata
{
    class Field
    {
        public Table Table { get; set; }
        public SqlField SqlField { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public bool IsKey { get; set; }
    }
}