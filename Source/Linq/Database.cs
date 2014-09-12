using Pagan.Configuration;
using Pagan.Relationships;

namespace Pagan.Linq
{
    public class Database
    {
        private readonly IDefinitionFactory _factory;

        public Database()
        {
            _factory = new DefinitionFactory();
        }

        public Query<T> Query<T>()
        {
            var definition = _factory.GetDefinition(typeof (T));
            var builder = new QueryBuilder(new RelationshipResolver(_factory));
            builder.AddSource(definition);
            return new Query<T>((T)definition.Instance, builder);
        }
        
    }
}