using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Tests.SampleDefinitions
{
    public class NoTwinExample
    {
        public Table Any { get; set; }
        public Key Id { get; set; }

        public Principal<User> User { get; set; } 
    }
}