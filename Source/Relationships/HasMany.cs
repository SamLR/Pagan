using Pagan.Registry;

namespace Pagan.Relationships
{
    public class HasMany<T> : ChildRef<T>
    {
        public HasMany(Table table, string name) : base(table, name)
        {
            ManyDependents = true;
        }
    }
}