namespace Pagan.Relationships
{
    public abstract class ChildRef<T> : LinkRef<T>, IPrincipal
    {
        protected ChildRef(Table table, string name) : base(table, name)
        {
        }

        public IDependent GetDependent()
        {
            return (IDependent)GetPartnerRef();
        }

        public bool ManyDependents { get; protected set; }

        public Column[] PrimaryKeyColumns { get { return Table.KeyColumns; } }


        protected override Relationship GetRelationship()
        {
            var dependent = GetDependent();
            EnsureForeignKey(this, dependent);
            return new Relationship(this, dependent, Role.Dependent);
        }
    }
}