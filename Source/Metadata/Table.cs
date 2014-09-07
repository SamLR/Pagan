using System;
using Pagan.SqlObjects;

namespace Pagan.Metadata
{
    class Table
    {
        public SqlTable SqlTable { get; set; }
        public Type EntityType { get; set; }
        public Field[] Fields { get; set; }

    }
}