using Elephant.Model;
using System.Text.RegularExpressions;

namespace Elephant_Services.TagDataFile.FileType;

public class UCNFile : XXFile, ITDCFile
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string[] FileContent { get; set; }
    public List<Tag> Tags { get; set; } = new List<Tag>();
    private const string patternCommand = @"(?-im)\AFN\sUCN_CP\s.*\sNODE_CP\s.*\sMODULE\s.*\sSLOT\s.\sENTITY\s.*\sENT_REF\s.*\sRN\s.*\sREF_MOD\s.*\sREF_SL\s.*";
    public static Regex RegexCommand = new(patternCommand, RegexOptions.Compiled);

    public UCNFile(string filePath)
    {
        FileName = Path.GetFileName(filePath);
        FilePath = filePath;
        FileContent = File.ReadAllLines(filePath);
        ColumnInfos = GetColumnsInformations(FileContent);
        GetTagsList();
    }

    public void GetTagsList()
    {
        try
        {
            if (ColumnInfos == null)
                return;

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
                    Tags.Add(tag);
                }
            }
        }
        catch (Exception)
        {
            return;
        }
    }
}