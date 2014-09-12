using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pagan.Conditions;
using Pagan.Configuration;
using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Linq
{
    class QueryBuilder : IQueryBuilder
    {
        private readonly SqlQuery _query;
        private readonly IRelationshipResolver _resolver;
        private JoinedTable _currentJoin;

        public QueryBuilder(IRelationshipResolver resolver)
        {
            _resolver = resolver;
            _query = new SqlQuery();
        }

        public void AddSource(IDefinition definition)
        {
            _query.Source.Definition = definition;
        }

        public void AddFilter(Condition condition)
        {
            if (_currentJoin == null)
            {
                if (_query.Condition == null) _query.Condition = condition;
                else _query.Condition &= condition;
            }
            else _currentJoin.JoinCondition &= condition;
        }

        public void AddSortAsc(Field field)
        {
            _query.Ordering.Add(new OrderedField {Direction = SortDirection.Ascending, Field = field});
        }

        public void AddSortDesc(Field field)
        {
            _query.Ordering.Add(new OrderedField { Direction = SortDirection.Descending, Field = field });
        }

        public void AddSelect(object selection)
        {
            HandleSelection(selection);
        }

        public void AddSelect(params SelectedField[] selection)
        {
            _query.Selection.AddRange(selection);
        }

        public void AddSelect(Field field, string name = null)
        {
            _query.Selection.Add(field.As(name));
        }

        public T Join<T>(Relationship<T> relationship)
        {
            var join = _resolver.GetJoin(relationship);

            _query.Source.JoinedTables.Add(join);
            _currentJoin = join;

            return (T) join.Definition.Instance;
        }

        private IEnumerable<IDefinition> Sources()
        {
            yield return _query.Source.Definition;
            foreach (var join in _query.Source.JoinedTables)
                yield return join.Definition;
        }

        private void HandleSelection(object selection)
        {
            var items = selection.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var item in items)
            {
                // members of type Field are added directly e.g. member is Field 'Id' from 'Users'
                if (typeof (Field).IsAssignableFrom(item.PropertyType))
                    AddSelect((Field) item.GetValue(selection, null), item.Name);

                // other member types are compared to the query sources, and all fields added on match
                // e.g member is 'Users', add 'Users.*'
                var source = Sources().FirstOrDefault(x => x.Type == item.PropertyType);
                if (source != null)
                    AddSelect(source.Fields.Select(f=> (SelectedField)f).ToArray());
            }
        }
    }
}