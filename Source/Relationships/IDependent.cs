using Pagan.DbComponents;

namespace Pagan.Relationships
{
    public interface IDependent
    {
        string Name { get; }
        bool OptionalParent { get; }
        Column[] ForeignKeyColumns { get; }
        void SetForeignKey(params Column[] fkColumns);
        IPrincipal GetPrincipal();
    }
}