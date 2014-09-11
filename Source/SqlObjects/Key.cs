namespace Pagan.SqlObjects
{
    public class Key : Field
    {
        public Key(string name) : base(name)
        {
            IsKey = true;
        }
    }
}