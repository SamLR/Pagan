using Pagan.Commands;
using Pagan.Queries;

namespace Pagan.Adapters
{
    class SqlDbAdapter : IDbAdapter
    {
        public IDbTranslation TranslateQuery(IQuery query)
        {
            return new SqlQueryTranslation(query);
        }

        public IDbTranslation TranslateCommand(ICommand command)
        {
            return new SqlCommandTranslation(command);
        }
    }
}