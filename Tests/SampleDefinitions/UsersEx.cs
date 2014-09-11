using Pagan.SqlObjects;

namespace Pagan.Tests.SampleDefinitions
{
    // Same as Users definition, except with a Configure method.
    // Also provide a Table property to map the name
    public class UsersEx: Users
    {
        public Schema Dbo { get; set; }
        public Table Users { get; set; }

        public void Configure()
        {
            FirstName.Name = "First_Name";
            LastName.Name = "Last_Name";
        }       
    }
}