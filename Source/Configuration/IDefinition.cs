using System;
using System.Collections.Generic;
using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Configuration
{
    interface IDefinition
    {
        Type Type { get; }
        object Instance { get; }
        Schema Schema { get; }
        Table Table { get; }
        List<Field> Fields { get; }
        List<Key> Keys { get; }
        List<Relationship> Relationships { get; }
    }
}