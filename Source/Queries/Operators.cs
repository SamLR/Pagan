namespace Pagan.Queries
{
    public enum Operators
    {
        Equal = 1,
        NotEqual = -1,
        GreaterThan = 2,
        LessOrEqual = -2,
        LessThan = 3,
        GreaterOrEqual = -3,

        And = 4,
        Or = -4,

        Like = 5,
        Unlike = -5,
        In = 6,
        NotIn = -6,
        IsNull = 7,
        IsNotNull = -7
    }
}
