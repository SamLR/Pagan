using Pagan.Metadata;

namespace Pagan.Linq
{
    public partial class Query<TSource>
    {
        private readonly ExpressionResolver _resolver;
        private readonly ITableFactory _factory;

        public Query()
        {
            _resolver = new ExpressionResolver();
            _factory = new TableFactory();
        }

    }
}
