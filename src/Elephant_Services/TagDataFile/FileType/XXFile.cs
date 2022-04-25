using Elephant.Model;
using System.Text.RegularExpressions;

namespace Elephant_Services.TagDataFile.FileType;

public abstract class XXFile
{
    public List<TagFileColumn>? ColumnInfos { get; set; }

    public int LineSize { get; set; } = 0;
    public const string LineRegex = @"(?-im)\ANET>.*";

    /// <summary>
    /// Create a list with the informations for each columns
    /// </summary>
    /// <returns></returns>
    public List<TagFileColumn>? GetColumnsInformations(string[] fileContent)
    {
        //(string names, string sizes) = GetHeaderLines() ?? default;
        var lineInfos = new List<TagFileColumn>();

        string? headerNames = GetHeaderNames(fileContent);
        string? headerSizes = GetHeaderSizes(fileContent);

        if (headerNames != null && headerSizes != null)
        {
            int headerPosition = 0;
            int startPosition = 0;
            string[] names = headerNames.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string[] sizes = headerSizes.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            LineSize = headerSizes.Length;

            foreach (string size in sizes)
            {
                TagFileColumn columnInfo = new()
                {
                    Name = names[headerPosition],
                    StartIndex = startPosition,
                    Length = size.Length
                };
                headerPosition++;
                startPosition = startPosition + size.Length + 1;
                lineInfos.Add(columnInfo);
            }

            return lineInfos;
        }
        return null;
    }


    /// <summary>
    /// Correct line size with spaces to match file header.
    /// </summary>
    /// <param name="line">Line to correct.</param>
    /// <returns></returns>
    public string CorrectLineSize(string line)
    {
        if (LineSize != line.Length)
        {
            line = line.PadRight(LineSize);
        }
        return line;
    }

    private string? GetHeaderNames(string[] fileContent)
    {
        if (fileContent == null)
            return null;

        const string regexHeaderName = @"(?-im)\A\s{1,}MEDIA\b\s{1,}";
        foreach (string line in fileContent)
        {
            if (Regex.IsMatch(line, regexHeaderName))
            {
                return line;
            }
        }

        return null;
    }

    private string? GetHeaderSizes(string[] fileContent)
    {
        const string regexHeaderSizes = @"\A-{2,}[\s-]*";
        foreach (string line in fileContent)
        {
            if (Regex.IsMatch(line, regexHeaderSizes))
            {
                return line;
            }
        }

        return null;
    }
}