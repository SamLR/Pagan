using Pagan.Registry;

namespace Pagan.Relationships
{
    public class WithOptional<T> : ParentRef<T>
    {
        public WithOptional(Table table, string name) : base(table, name)
        {
            OptionalParent = true;
        }
    }
}