using System;

namespace Pagan.Metadata
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SchemaNameAttribute : Attribute
    {
        public SchemaNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}