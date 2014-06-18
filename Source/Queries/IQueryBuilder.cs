namespace Pagan.Queries
{
    public interface IQueryBuilder
    {
        Query Select(params QueryColumn[] selected);
        Query OrderBy(params SortingColumn[] sorting);
        Query Where(FilterExpression filter);
    }
}