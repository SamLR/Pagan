using System.Collections.Generic;

namespace Pagan.Queries
{
    public interface IQuery: IQuerySource
    {
        IEnumerable<QueryColumn> Participants { get; }
        IEnumerable<SortingColumn> Sorting { get; }
        FilterExpression Filter { get; }
        IEnumerable<IQuery> JoinedQueries { get; }
    }
}