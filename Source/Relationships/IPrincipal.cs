namespace Pagan.Relationships
{
    public interface IPrincipal: ILinkRef
    {
        IDependent GetDependent();

        bool ManyDependents { get; }
        Column[] PrimaryKeyColumns { get; }
    }
}