using Elephant.Model;

namespace Elephant.Services.TagDataFile.FileType;

public class HMGRPFile : XXFile, ITDCFile
{
    private const string _patternCommand = @"(?-im)\AFN\sAREA\s.*\sENTITY\s.*\sENT_REF\s.*";
    public static Regex RegexCommand = new(_patternCommand, RegexOptions.Compiled);
    public HMGRPFile(string filePath) : base(filePath) { }

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
                    var name = ColumnInfos.First(c => c.Name == "ENTITY");
                    var value = ColumnInfos.First(c => c.Name == "ENT_REF");

                    string lineCorrected = CorrectLineSize(line);

                    var tag = new Tag()
                    {
                        Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                        Value = lineCorrected.Substring(value.StartIndex, value.Length).Trim(),
                        Parameter = "ENT_REF",
                        Origin = "HMGRP"
                    };

                    tags.Add(tag);
                }
            }

            return tags;
        }
        catch (Exception)
        {
            System.Windows.MessageBox.Show($"Erreur de lecture du fichier {FileName}");
            return null;
        }
    }
}
