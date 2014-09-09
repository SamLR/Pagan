using Pagan.SqlObjects;

namespace Pagan.Tests.Configuration
{
    public class User
    {
        public Schema Dbo { get; set; }
        public Table Users { get; set; }
        public Key Id { get; set; }
        public Field FirstName { get; set; }
        public Field LastName { get; set; }
        public Field Email { get; set; }
    }

    public class UserEx: User
    {
        public void Configure()
        {
            FirstName.Name = "First_Name";
            LastName.Name = "Last_Name";
            Users.UseSingularForm();
        }       
    }

    public class NoKeyExample
    {
        public Table Table { get; set; }
        public Field Name { get; set; }
    }

    public class NoTableExample
    {
        public Key Id { get; set; }
    }
}