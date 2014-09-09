namespace Pagan.Configuration
{
    interface IDefinitionFactory
    {
        Definition<T> GetDefinition<T>();
    }
}