using Pagan.Registry;

namespace Pagan.Relationships
{
    public class HasMany<T> : ChildRef<T>
    {
        public HasMany(Controller controller, string name) : base(controller, name)
        {
            ManyDependents = true;
        }
    }
}