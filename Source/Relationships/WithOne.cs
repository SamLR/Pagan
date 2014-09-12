namespace Pagan.Relationships
{
    public class WithOne<T> : Dependent<T> where T : class,new()
    {
        public WithOne(string name) : base(name)
        {
            Multiplicity = Multiplicity.One;
        }
    }
}