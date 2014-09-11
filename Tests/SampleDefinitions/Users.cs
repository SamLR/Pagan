using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Tests.SampleDefinitions
{
    public class Users
    {
        public Key Id { get; set; }
        public Field FirstName { get; set; }
        public Field LastName { get; set; }
        public Field Email { get; set; }

        public HasMany<Blogs> Blogs { get; set; } 
    }
}