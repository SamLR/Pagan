namespace Pagan.Relationships
{
    public class HasOne<T> : ChildRef<T>
    {
        public HasOne(Table table, string name) : base(table, name)
        {
            ManyDependents = false;
        }
    }
}