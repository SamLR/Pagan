namespace Pagan.Relationships
{
    public class WithOne<T> : ParentRef<T>
    {
        public WithOne(Table table, string name) : base(table, name)
        {
            OptionalParent = false;
        }
    }
}