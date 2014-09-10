namespace Pagan.Tests.SampleDefinitions
{
    // Same as User definition, except with a Configure method
    public class UserEx: User
    {
        public void Configure()
        {
            FirstName.Name = "First_Name";
            LastName.Name = "Last_Name";
            Users.UseSingularForm();
        }       
    }
}