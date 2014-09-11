namespace Pagan.Relationships
{
    public class WithOptional<T> : Dependent<T>
    {
        public WithOptional(string name): base(name)
        {
            Multiplicity = Multiplicity.Optional;
        }
    }
}