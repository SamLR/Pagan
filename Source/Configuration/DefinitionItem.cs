using System;

namespace Pagan.Configuration
{
    public class DefinitionItem
    {
        internal DefinitionItem(string memberName, Definition definition)
        {
            Name = memberName;
            MemberName = memberName;
            Definition = definition;
        }

        internal string MemberName;
        internal Definition Definition;

        private string _name;

        public string Name
        {
            get { return String.IsNullOrEmpty(_name) ? MemberName : _name; }
            set { _name = value; }
        }
    }
}