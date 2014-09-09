using System;
using Pagan.Configuration;
using Pagan.SqlObjects;

namespace Pagan.Relationships
{
    public class Dependent<T> : Relationship<T>
    {
        internal Dependent(string memberName, Definition definition) : base(memberName, definition)
        {
        }

        public Dependent<T> Map(Field fKey, Func<T, Key> keySelector)
        {
            Mappings[fKey] = keySelector;
            return this;
        }
    }
}