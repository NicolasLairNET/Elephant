using Elephant.Model;
using System.Text.RegularExpressions;

namespace Elephant_Services.TagDataFile.FileType;

public class UCNFile : XXFile, ITDCFile
{
    private const string _patternCommand = @"(?-im)\AFN\sUCN_CP\s.*\sNODE_CP\s.*\sMODULE\s.*\sSLOT\s.\sENTITY\s.*\sENT_REF\s.*\sRN\s.*\sREF_MOD\s.*\sREF_SL\s.*";
    public static Regex RegexCommand = new(_patternCommand, RegexOptions.Compiled);
    public UCNFile(string filePath) : base(filePath) { }

    public List<Tag>? GetTagsList()
    {
        try
        {
            List<Tag> tags = new();

            if (ColumnInfos == null)
            {
                return tags;
            }

            foreach (var line in FileContent)
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
                        Origin = "UCN"
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