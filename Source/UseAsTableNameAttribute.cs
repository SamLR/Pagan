using System;

namespace Pagan
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false,Inherited=true)]
    public class UseAsTableNameAttribute: Attribute
    {
    }
}
