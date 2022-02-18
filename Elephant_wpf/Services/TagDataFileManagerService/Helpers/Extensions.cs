namespace Elephant.Services.TagDataFileManagerService.Helpers;

public static class Extensions
{
    public static string RegexFormat(this string value)
    {
        value += "*".Replace("**", "*");
        return '^' + value
        .Replace(")", "")
        .Replace("(", "")
        .Replace('?', '.')
        .Replace("*", ".*") + "$";
    }
}
