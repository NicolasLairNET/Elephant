namespace Elephant_Models.Helpers;

public static class Extensions
{
    public static string RegexFormat(this string value)
    {
        value += "*".Replace("**", "*");
        return @"\A" + value
        .Replace(")", "")
        .Replace("(", "")
        .Replace('?', '.')
        .Replace("*", ".*");
    }
}
