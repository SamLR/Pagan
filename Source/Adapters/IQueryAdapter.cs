using System.Data.Common;
using Pagan.Commands;
using Pagan.Queries;

namespace Pagan.Adapters
{
    public interface IQueryAdapter
    {
        void TranslateQuery(Query query, DbCommand dbCommand);
        void TranslateCommand(Command paganCommand, DbCommand dbCommand);
    }
}