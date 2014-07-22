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

        StartsWith = 5,
        NotStartsWith = -5,
        EndsWith = 6,
        NotEndsWith = -6,
        Contains = 7,
        NotContains = -7,
        In = 8,
        NotIn = -8,
        IsNull = 9,
        IsNotNull = -9
    }
}
