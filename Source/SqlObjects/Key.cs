using Pagan.Configuration;

namespace Pagan.SqlObjects
{
    public class Key : Field
    {
        internal Key(string memberName, Definition definition) : base(memberName, definition)
        {
            IsKey = true;
        }
    }
}