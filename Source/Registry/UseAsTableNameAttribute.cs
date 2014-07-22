using System;

namespace Pagan.Registry
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false,Inherited=true)]
    public class UseAsTableNameAttribute: Attribute
    {
    }
}
