using Elephant.Model;
using System.Text.RegularExpressions;

namespace Elephant_Services.TagDataFile.FileType;

public class HWYFile : XXFile, ITDCFile
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string[] FileContent { get; set; }
    public List<Tag> Tags { get; set; } = new List<Tag>();
    private const string patternCommand = @"(?-im)\AFN\s+HWY_CP\s.*\sENTITY\s.*\sENT_REF\s.*";
    public static Regex RegexCommand = new(patternCommand, RegexOptions.Compiled);
    public HWYFile(string filePath)
    {
        FileName = Path.GetFileName(filePath);
        FilePath = filePath;
        FileContent = File.ReadAllLines(filePath);
        ColumnInfos = GetColumnsInformations(FileContent);
        GetTagsList();
    }

    public void GetTagsList()
    {
        List<Tag> tags = new();

        if (ColumnInfos == null)
            return;

        foreach (string line in FileContent)
        {
            try
            {
                if (Regex.IsMatch(line, LineRegex))
                {
                    var name = ColumnInfos.First(column => column.Name == "ENTITY");
                    var value = ColumnInfos.First(column => column.Name == "ENT_REF");

                    string lineCorrected = CorrectLineSize(line);

                    var tag = new Tag()
                    {
                        Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                        Value = lineCorrected.Substring(value.StartIndex, value.Length).Trim(),
                        Parameter = "ENT_REF",
                        Origin = "HIWAY"
                    };
                    Tags.Add(tag);
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}