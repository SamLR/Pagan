namespace Pagan.Relationships
{
    public class HasOne<T> : Principal<T>
    {
        public HasOne(string name) : base(name)
        {
            Multiplicity = Multiplicity.One;
        }
    }
}