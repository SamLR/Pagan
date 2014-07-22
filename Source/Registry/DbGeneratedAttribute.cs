using System;

namespace Pagan.Registry
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DbGeneratedAttribute : Attribute
    {
    }
}