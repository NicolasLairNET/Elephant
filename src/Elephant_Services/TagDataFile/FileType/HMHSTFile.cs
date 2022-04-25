using Elephant.Model;
using System.Text.RegularExpressions;

namespace Elephant_Services.TagDataFile.FileType;

public class HMHSTFile : XXFile, ITDCFile
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string[] FileContent { get; set; }
    public List<Tag> Tags { get; set; } = new List<Tag>();
    private const string patternCommand = @"(?-im)\AFN\sH_UNIT\s.*\sH_GRP\s.*\sENT_REF\s.*";
    public static Regex RegexCommand = new(patternCommand, RegexOptions.Compiled);
    public HMHSTFile(string filePath)
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

            foreach (string line in FileContent)
            {
                if (Regex.IsMatch(line, LineRegex))
                {
                    var name = ColumnInfos.First(c => c.Name == "HU");
                    var value = ColumnInfos.First(c => c.Name == "HGRP");

                    string lineCorrected = CorrectLineSize(line);

                    var tag = new Tag()
                    {
                        Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                        Value = lineCorrected.Substring(value.StartIndex, value.Length).Trim(),
                        Parameter = "HGRP",
                        Origin = "HMHST"
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