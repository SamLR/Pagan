using Pagan.Conditions;
using Pagan.Configuration;
using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Linq
{
    interface IQueryBuilder
    {
        void AddSource(IDefinition defnDefinition);
        void AddFilter(Condition condition);
        void AddSortAsc(Field field);
        void AddSortDesc(Field field);
        void AddSelect(object selection);
        void AddSelect(params SelectedField[] selection);
        T Join<T>(Relationship<T> relationship);

    }
}