using Pagan.SqlObjects;

namespace Pagan.Tests.SampleDefinitions
{
    /// <summary>
    /// Same as Users definition, except with a Configure method.
    /// Also provides a Table property to map the table name, and an explicit Schema property.
    /// </summary>
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