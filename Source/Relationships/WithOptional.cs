namespace Pagan.Relationships
{
    public class WithOptional<T> : Dependent<T> where T : class,new()
    {
        public WithOptional(string name): base(name)
        {
            Multiplicity = Multiplicity.Optional;
        }
    }
}