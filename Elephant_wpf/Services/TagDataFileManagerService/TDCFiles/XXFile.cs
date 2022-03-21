using Elephant.Model;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public abstract class XXFile
{
    public List<ColumnInfo>? ColumnInfos { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public string[] FileContent { get; set; }
    public int LineSize { get; set; } = 0;
    public const string LineRegex = @"(?-im)\ANET>.*";

    public XXFile(string filePath)
    {
        FilePath = filePath;
        FileName = Path.GetFileName(filePath);
        FileContent = File.ReadAllLines(filePath);
        ColumnInfos = GetColumnsInformations();
    }

    /// <summary>
    /// Create a list with the informations for each columns
    /// </summary>
    /// <returns></returns>
    public List<ColumnInfo>? GetColumnsInformations()
    {
        //(string names, string sizes) = GetHeaderLines() ?? default;
        var lineInfos = new List<ColumnInfo>();

        string? headerNames = GetHeaderNames();
        string? headerSizes = GetHeaderSizes();

        if (headerNames != null && headerSizes != null)
        {
            int headerPosition = 0;
            int startPosition = 0;
            string[] names = headerNames.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string[] sizes = headerSizes.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            LineSize = headerSizes.Length;

            foreach (string size in sizes)
            {
                ColumnInfo columnInfo = new()
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

    private string? GetHeaderNames()
    {
        const string regexHeaderName = @"(?-im)\A\s{1,}MEDIA\b\s{1,}";
        foreach (string line in FileContent)
        {
            if (Regex.IsMatch(line, regexHeaderName))
            {
                return line;
            }
        }

        return null;
    }

    private string? GetHeaderSizes()
    {
        const string regexHeaderSizes = @"\A-{2,}[\s-]*";
        foreach (string line in FileContent)
        {
            if (Regex.IsMatch(line, regexHeaderSizes))
            {
                return line;
            }
        }

        return null;
    }
}