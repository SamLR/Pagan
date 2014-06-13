using Pagan.Registry;

namespace Pagan.Relationships
{
    public class WithOne<T> : ParentRef<T>
    {
        public WithOne(Controller controller, string name) : base(controller, name)
        {
            OptionalParent = false;
        }
    }
}