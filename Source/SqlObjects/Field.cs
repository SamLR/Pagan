using System;
using Pagan.Configuration;

namespace Pagan.SqlObjects
{
    public partial class Field: IDefinitionItem
    {
        private string _name;

        public Field(string name)
        {
            MemberName = name;
            _name = name;
        }

        public Table Table { get; internal set; }
        public string MemberName { get; private set; }

        public string Name
        {
            get { return String.IsNullOrEmpty(_name) ? MemberName: _name; }
            set { _name = value; }
        }

        public bool IsKey { get; protected set; }

        public SelectedField As(string name)
        {
            return name != null && !name.Equals(MemberName)
                ? new SelectedField {Alias = name, Field = this}
                : this;

        }
    }
}