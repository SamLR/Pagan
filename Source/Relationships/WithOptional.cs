using Pagan.Registry;

namespace Pagan.Relationships
{
    public class WithOptional<T> : ParentRef<T>
    {
        public WithOptional(Controller controller, string name) : base(controller, name)
        {
            OptionalParent = true;
        }
    }
}