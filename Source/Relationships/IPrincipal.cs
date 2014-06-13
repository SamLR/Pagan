namespace Pagan.Relationships
{
    public interface IPrincipal
    {
        string Name { get; }
        bool ManyDependents { get; }
        Column[] PrimaryKeyColumns { get; }
        IDependent GetDependent();
    }
}