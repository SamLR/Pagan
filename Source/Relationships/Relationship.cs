using System.Linq;
using Pagan.Queries;

namespace Pagan.Relationships
{
    public class Relationship
    {
        public Relationship(IPrincipal principal, IDependent dependent, Role role)
        {
            Principal = principal;
            Dependent = dependent;
            Role = role;
            Name = Role == Role.Dependent ? Principal.Name : Dependent.Name;
            JoinExpression = CreateJoinExpression(principal.PrimaryKeyColumns, dependent.ForeignKeyColumns);
        }

        public IDependent Dependent { get; private set; }
        public IPrincipal Principal { get; private set; }
        public FilterExpression JoinExpression { get; internal set; }
        public Role Role { get; private set; }
        public string Name { get; private set; }

        private static FilterExpression CreateJoinExpression(Column[] primaryKey, Column[] foreignKey)
        {
            return primaryKey.Select((t, i) => new FilterExpression(t, foreignKey[i], Operators.Equal)).All();
        }
    }
}
