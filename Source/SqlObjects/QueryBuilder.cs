namespace Pagan.SqlObjects
{
    class QueryBuilder
    {
        private readonly SqlQuery _query;

        public QueryBuilder(Table table)
        {
            _query = new SqlQuery(table);
        }
    }
}