using System;

static class ComparisonExtensions
{
    public static Comparison<T> Reversed<T>(this Comparison<T> baseCompare)
    {
        return (left, right) => baseCompare(right, left);
    }
}
