using Elephant.Model;
using System.Text.RegularExpressions;

namespace Elephant_Services.TagDataFile.FileType;
public class CLAMFile : XXFile, ITDCFile
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string[] FileContent { get; set; }
    public List<Tag> Tags { get; set; } = new List<Tag>();

    private const string PatternCommand = @"(?-im)\AFN\s+AM_CP\s.*\sENTITY\s.*\sCL.*\sENT_REF\s.*";
    public static Regex RegexCommand = new(PatternCommand, RegexOptions.Compiled);
    public CLAMFile(string filePath)
    {
        FileName = Path.GetFileName(filePath);
        FilePath = filePath;
        FileContent = File.ReadAllLines(filePath);
        ColumnInfos = GetColumnsInformations(FileContent);
        GetTagsList();
    }

    public void GetTagsList()
    {
        if (ColumnInfos == null)
            return;

        foreach (string line in FileContent)
        {
            if (Regex.IsMatch(line, LineRegex))
            {
                var name = ColumnInfos.First(column => column.Name == "ENTITY");
                var value = ColumnInfos.First(column => column.Name == "ENT_REF");
                var valueCl = ColumnInfos.First(column => column.Name == "CL");

                string lineCorrected = CorrectLineSize(line);

                var tagCl = new Tag()
                {
                    Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                    Value = lineCorrected.Substring(valueCl.StartIndex, valueCl.Length).Trim(),
                    Parameter = "CL",
                    Origin = "CLAM"
                };

                var tag = new Tag()
                {
                    Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                    Value = lineCorrected.Substring(value.StartIndex, value.Length).Trim(),
                    Parameter = "ENT_REF",
                    Origin = "CLAM"
                };

                Tags.Add(tagCl);
                Tags.Add(tag);
            }
        }
    }
}