using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Tests.SampleDefinitions
{
    public class User
    {
        public Schema Dbo { get; set; }
        public Table Users { get; set; }
        public Key Id { get; set; }
        public Field FirstName { get; set; }
        public Field LastName { get; set; }
        public Field Email { get; set; }

        public Principal<Blog> Blogs { get; set; } 
    }
}