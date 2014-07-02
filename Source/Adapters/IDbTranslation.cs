using System.Collections.Generic;

namespace Pagan.Adapters
{
    public interface IDbTranslation
    {
        string GetCommandText();
        IDictionary<string, object> Parameters { get; }
    }
}