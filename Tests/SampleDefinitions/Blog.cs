using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Tests.SampleDefinitions
{
    public class Blog
    {
        public Table Blogs { get; set; }
        public Key Id { get; set; }
        public Field Name { get; set; }
        public Field UserId { get; set; }

        public Dependent<User> User { get; set; }

        public void Configure()
        {
            User.Map(UserId, x => x.Id).HasMany();
        }
    }
}