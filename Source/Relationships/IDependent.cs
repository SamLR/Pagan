namespace Pagan.Relationships
{
    public interface IDependent
    {
        Table Table { get; }
        string Name { get; }
        IPrincipal GetPrincipal();
        
        bool OptionalParent { get; }
        Column[] ForeignKeyColumns { get; }
        void SetForeignKey(params Column[] fkColumns);
    }
}