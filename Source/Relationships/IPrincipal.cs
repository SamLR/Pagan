namespace Pagan.Relationships
{
    public interface IPrincipal
    {
        Table Table { get; }
        string Name { get; }
        IDependent GetDependent();

        bool ManyDependents { get; }
        Column[] PrimaryKeyColumns { get; }
    }
}