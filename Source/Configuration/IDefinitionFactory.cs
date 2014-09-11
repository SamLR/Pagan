using System;

namespace Pagan.Configuration
{
    interface IDefinitionFactory
    {
        IDefinition GetDefinition(Type definitionType);
    }
}