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
                if (line.Contains("NET"))
                {
                    var hu = ColumnInfos.First(c => c.Name == "HU");
                    var hgrp = ColumnInfos.First(c => c.Name == "HGRP");

                    var tag = new TDCTag()
                    {
                        Name = line.Substring(hu.StartIndex, hu.Length).Trim(),
                        Value = line.Substring(hgrp.StartIndex, hgrp.Length).Trim(),
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