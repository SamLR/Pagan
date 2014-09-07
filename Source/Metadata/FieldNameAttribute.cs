using System;

namespace Pagan.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FieldNameAttribute : Attribute
    {
        public FieldNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}