namespace Pagan.Relationships
{
    public class WithOne<T> : Dependent<T>
    {
        public WithOne(string name) : base(name)
        {
            Multiplicity = Multiplicity.One;
        }
    }
}