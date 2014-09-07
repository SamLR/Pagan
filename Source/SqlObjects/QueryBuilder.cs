namespace Pagan.SqlObjects
{
    class QueryBuilder
    {
        private readonly SqlQuery _query;

        public QueryBuilder(SqlTable table)
        {
            _query = new SqlQuery(table);
        }
    }
}