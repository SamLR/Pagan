using System;
using System.Collections.Generic;

namespace Pagan.Configuration
{
    internal class DefinitionFactory: IDefinitionFactory
    {
        private readonly Dictionary<Type, IDefinition> _cache;

        internal DefinitionFactory()
        {
            _cache = new Dictionary<Type, IDefinition>();
        }

        public IDefinition GetDefinition(Type definitionType)
        {
            IDefinition definition;

            if (!_cache.TryGetValue(definitionType, out definition))
                _cache[definitionType] = definition = new Definition(definitionType);

            return definition;
        }
    }
}