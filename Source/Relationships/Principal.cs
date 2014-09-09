using System;
using Pagan.Configuration;
using Pagan.SqlObjects;

namespace Pagan.Relationships
{
    public class Principal<T> : Relationship<T>
    {
        internal Principal(string memberName, Definition definition) : base(memberName, definition)
        {
        }

        public Principal<T> Map(Key key, Func<T, Field> fkSelector)
        {
            Mappings[key] = fkSelector;
            return this;
        }
    }
}