using Elephant.Model;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class HMHSTFile : XXFile, ITDCFile
{
    public const string CommandRegex = @"(?-im)\AFN\sH_UNIT\s.*\sH_GRP\s.*\sENT_REF\s.*";
    public HMHSTFile(string filePath) : base(filePath) { }

    public List<TDCTag>? GetTagsList()
    {
        try
        {
            List<TDCTag> tags = new();

            if (ColumnInfos == null)
            {
                return null;
            }

            foreach (string line in FileContent)
            {
                if (Regex.IsMatch(line, LineRegex))
                {
                    var name = ColumnInfos.First(c => c.Name == "HU");
                    var value = ColumnInfos.First(c => c.Name == "HGRP");

                    string lineCorrected = CorrectLineSize(line);

                    var tag = new TDCTag()
                    {
                        Name = lineCorrected.Substring(name.StartIndex, name.Length).Trim(),
                        Value = lineCorrected.Substring(value.StartIndex, value.Length).Trim(),
                        Parameter = "HGRP",
                        Origin = "HMHST"
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