namespace Elephant.Helpers;

public static class Extensions
{
    public static string RegexFormat(this string value)
    {
        return '^' + value
        .Replace(")", "")
        .Replace("(", "")
        .Replace('?', '.')
        .Replace("*", ".*") + "$";
    }
}
