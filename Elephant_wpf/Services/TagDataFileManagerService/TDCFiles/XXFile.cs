using Elephant.Model;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public abstract class XXFile
{
    public List<ColumnInfo>? ColumnInfos { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public string[] FileContent { get; set; }

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

        string[]? headerNames = GetHeaderName();
        string[]? headerSizes = GetHeaderSizes();

        if (headerNames != null && headerSizes != null)
        {
            int headerPosition = 0;
            int startPosition = 0;

            foreach (string line in headerSizes)
            {
                ColumnInfo columnInfo = new()
                {
                    Name = headerNames[headerPosition],
                    StartIndex = startPosition,
                    Length = line.Length
                };
                headerPosition++;
                startPosition = startPosition + line.Length + 1;
                lineInfos.Add(columnInfo);
            }

            return lineInfos;
        }
        return null;
    }

    private string[]? GetHeaderName()
    {
        const string regexHeaderName = @"(?-im)\A\s{1,}MEDIA\b\s{1,}";
        foreach (string line in FileContent)
        {
            if (Regex.IsMatch(line, regexHeaderName))
            {
                return line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            }
        }

        return null;
    }

    private string[]? GetHeaderSizes()
    {
        const string regexHeaderSizes = @"\A-{2,}[\s-]*";
        foreach (string line in FileContent)
        {
            if (Regex.IsMatch(line, regexHeaderSizes))
            {
                return line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            }
        }

        return null;
    }
}