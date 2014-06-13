using System.Linq;
using Pagan.DbComponents;
using Pagan.Registry;

namespace Pagan.Relationships
{
    public abstract class ChildRef<T> : LinkRef<T>, IPrincipal
    {
        protected ChildRef(Controller controller, string name) : base(controller, name)
        {
            PrimaryKeyColumns = Controller.KeyColumns;
        }

        public bool ManyDependents { get; protected set; }
        public Column[] PrimaryKeyColumns { get; private set; }
        public IDependent GetDependent()
        {
            return (IDependent) GetPartnerRef();
        } 
    }
}