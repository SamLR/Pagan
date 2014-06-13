using Pagan.DbComponents;
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
            JoinExpression = CreateJoinExpression(principal.PrimaryKeyColumns, dependent.ForeignKeyColumns);
        }

        public IDependent Dependent { get; private set; }
        public IPrincipal Principal { get; private set; }
        public FilterExpression JoinExpression { get; private set; }
        public Role Role { get; private set; }

        private static FilterExpression CreateJoinExpression(Column[] primaryKey, Column[] foreignKey)
        {
            //todo: Create join expression
            return new FilterExpression();
        }
    }
}
