using System.Data.Common;
using Pagan.Queries;

namespace Pagan.Adapters
{
    public interface IQueryAdapter
    {
        DbCommand GetCommand(Query query, DbConnection connection);
    }
}