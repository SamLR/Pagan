namespace Pagan.Relationships
{
    public class HasOne<T> : Principal<T> where T : class,new()
    {
        public HasOne(string name) : base(name)
        {
            Multiplicity = Multiplicity.One;
        }
    }
}