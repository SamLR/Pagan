using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Tests.SampleDefinitions
{
    public class NoTwinExample
    {
        public Key Id { get; set; }

        public HasMany<Users> Users { get; set; } 
    }
}