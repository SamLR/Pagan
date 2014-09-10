using System;

namespace Pagan.Configuration
{
    internal class Definition<T> : Definition
    {
        internal Definition(IDefinitionFactory factory): base(factory)
        {
            Type = typeof(T);
            Instance = Activator.CreateInstance<T>();
        }

        public T Instance { get; private set; }
    }
}
