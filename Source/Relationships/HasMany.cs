namespace Pagan.Relationships
{
    public class HasMany<T> : Principal<T>
    {
        public HasMany(string name) : base(name)
        {
            Multiplicity = Multiplicity.Many;
        }
    }
}