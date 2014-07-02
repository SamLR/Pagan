using System.Data.Common;
using Pagan.Commands;
using Pagan.Queries;

namespace Pagan.Adapters
{
    public interface IDbAdapter
    {
        IDbTranslation TranslateQuery(IQuery query);
        IDbTranslation TranslateCommand(ICommand command);
    }
}