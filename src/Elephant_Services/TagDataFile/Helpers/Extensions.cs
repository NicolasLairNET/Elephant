namespace Elephant_Services.TagDataFile.Helpers;

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
