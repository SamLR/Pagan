using System.Data;

namespace Pagan.Adapters
{
    public interface IDbTranslation
    {
        void BuildCommand(IDbCommand cmd);
    }
}