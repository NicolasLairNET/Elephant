using Elephant.Model;
using System.Text.RegularExpressions;

namespace Elephant_Services.TagDataFile.FileType;

public class PEFile : XXFile, ITDCFile
{
    private const string _patternCommand = @"(?-im)\AFN\s+PE\s.*\sENT_REF\s.*";
    public static Regex RegexCommand = new(_patternCommand, RegexOptions.Compiled);
    public PEFile(string filePath) : base(filePath) { }

    public List<Tag>? GetTagsList()
    {
        try
        {
            List<Tag> tags = new();

            if (ColumnInfos == null)
            {
                return null;
            }

            foreach (string line in FileContent)
            {
                if (Regex.IsMatch(line, LineRegex))
                {
                    var name = ColumnInfos.First(c => c.Name == "PE");
                    var value = ColumnInfos.First(c => c.Name == "ENT_REF");

                    string lineCorrected = CorrectLineSize(line);

                    var tag = new Tag()
                    {
                        Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                        Value = lineCorrected.Substring(value.StartIndex, value.Length).Trim(),
                        Parameter = "ENT_REF",
                        Origin = "PE"
                    };

                    tags.Add(tag);
                }
            }

            return tags;
        }
        catch (Exception)
        {
            return null;
        }
    }
}