using System;

namespace Pagan
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false,Inherited=true)]
    public class DbNameAttribute : Attribute
    {
        public DbNameAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}