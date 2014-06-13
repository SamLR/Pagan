namespace Pagan.Relationships
{
    public abstract class ChildRef<T> : LinkRef<T>, IPrincipal
    {
        protected ChildRef(Table table, string name) : base(table, name)
        {
            PrimaryKeyColumns = Table.KeyColumns;
        }

        public bool ManyDependents { get; protected set; }
        public Column[] PrimaryKeyColumns { get; private set; }
        public IDependent GetDependent()
        {
            return (IDependent) GetPartnerRef();
        } 
    }
}