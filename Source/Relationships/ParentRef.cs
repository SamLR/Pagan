namespace Pagan.Relationships
{
    public abstract class ParentRef<T> : LinkRef<T>, IDependent
    {
        protected ParentRef(Table table, string name) : base(table, name)
        {          
        }

        public bool OptionalParent { get; protected set; }
        public Column[] ForeignKeyColumns { get; private set; }
        public void SetForeignKey(params Column[] fkColumns)
        {
            ForeignKeyColumns = fkColumns;
        }
        public IPrincipal GetPrincipal()
        {
            return (IPrincipal) GetPartnerRef();
        }

        protected override Relationship GetRelationship()
        {
            var principal = GetPrincipal();
            EnsureForeignKey(principal, this);
            return new Relationship(principal, this, Role.Dependent);
        }
    }
}