namespace Pagan.Relationships
{
    public class HasMany<T> : Principal<T> where T : class,new()
    {
        public HasMany(string name) : base(name)
        {
            Multiplicity = Multiplicity.Many;
        }
    }
}