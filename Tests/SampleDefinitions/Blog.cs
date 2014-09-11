using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Tests.SampleDefinitions
{
    public class Blogs
    {
        public Key Id { get; set; }
        public Field Name { get; set; }
        public Field UserId { get; set; }

        public WithOne<Users> User { get; set; }

        public void Configure()
        {
            User.Map(UserId, user => user.Id);
        }
    }
}