using Pagan.Registry;
using Pagan.Relationships;

namespace Pagan.Queries
{
    public class Query
    {
        private readonly Table _table;

        public Query(Table table)
        {
            _table = table;
        }

        public Relationship Relationship { get; internal set; }
    }
}
