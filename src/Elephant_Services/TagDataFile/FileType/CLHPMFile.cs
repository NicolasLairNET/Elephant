using Elephant.Model;
using System.Text.RegularExpressions;

namespace Elephant_Services.TagDataFile.FileType;

public class CLHPMFile : XXFile, ITDCFile
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string[] FileContent { get; set; }
    public List<Tag> Tags { get; set; } = new List<Tag>();

    private const string patternCommand = @"(?-im)\AFN\s+UCN_CP\s.*\sPM\s.\sPMOD_ENT\s.*PM_SEQ\s.*\sENT_REF\s.*\sRN\s.*\sREF_MOD\s.*\sREF_SL\s.*";
    public static Regex RegexCommand = new(patternCommand, RegexOptions.Compiled);
    public CLHPMFile(string filePath)
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
                var name = ColumnInfos.First(c => c.Name == "PMOD_ENT");
                var valuePm = ColumnInfos.First(c => c.Name == "PM_SEQ");
                var value = ColumnInfos.First(c => c.Name == "ENT_REF");

                string lineCorrected = CorrectLineSize(line);

                var tagPm = new Tag()
                {
                    Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                    Value = lineCorrected.Substring(valuePm.StartIndex, valuePm.Length).Trim(),
                    Parameter = "PM_SEQ",
                    Origin = "CLHPM"
                };

                var tag = new Tag()
                {
                    Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                    Value = lineCorrected.Substring(value.StartIndex, value.Length).Trim(),
                    Parameter = "ENT_REF",
                    Origin = "CLHPM"
                };

                Tags.Add(tagPm);
                Tags.Add(tag);
            }
        }
    }
}