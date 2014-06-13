using Pagan.Registry;

namespace Pagan.Relationships
{
    public class HasOne<T> : ChildRef<T>
    {
        public HasOne(Controller controller, string name) : base(controller, name)
        {
            ManyDependents = false;
        }
    }
}