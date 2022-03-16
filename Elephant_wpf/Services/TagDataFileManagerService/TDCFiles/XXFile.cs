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
        (string names, string sizes) = GetHeaderLines() ?? default;
        var lineInfos = new List<ColumnInfo>();

        if (names.Length == 0 || sizes.Length == 0)
        {
            return null;
        }

        string[] headerNames = names.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        string[] headerSizes = sizes.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        int headerPosition = 0;
        int startPosition = 0;

        for (int i = 0; i < headerSizes.Length; i++)
        {
            string? size = headerSizes[i];
            ColumnInfo columnInfo = new()
            {
                Name = headerNames[headerPosition],
                StartIndex = startPosition,
                Length = headerSizes[i].Length
            };
            headerPosition++;
            startPosition = startPosition + headerSizes[i].Length + 1;
            lineInfos.Add(columnInfo);
        }

        return lineInfos;
    }

    /// <summary>
    /// Get the two header lines.
    /// The first line is the name of the column
    /// the second line represents the size of the columns in the form of a dash.
    /// Exemple : ---- is a column of 4 characters.
    /// </summary>
    /// <returns></returns>
    private (string, string)? GetHeaderLines()
    {
        for (int i = 0; i < FileContent.Length; i++)
        {
            if (FileContent[i].Contains("MEDIA"))
            {
                return (FileContent[i], FileContent[i + 1]);
            }
        }
        return null;
    }
}