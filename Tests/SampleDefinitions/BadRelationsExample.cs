using Pagan.Relationships;
using Pagan.SqlObjects;

namespace Pagan.Tests.SampleDefinitions
{
    /// <summary>
    /// Demonstrates relationship error scenarios
    /// The Relationshp to Users has no mapping here or on Users so will throw an
    /// undefined mapping error.
    /// </summary>
    public class BadRelationsExample
    {
        public Key Id { get; set; }

        public HasMany<Users> Users { get; set; } 
    }
}