namespace Pagan.Relationships
{
    public interface IDependent: ILinkRef
    {
        IPrincipal GetPrincipal();
        
        bool OptionalParent { get; }
        Column[] ForeignKeyColumns { get; }
        void SetForeignKey(params Column[] fkColumns);
    }
}