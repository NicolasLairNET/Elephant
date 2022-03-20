using Elephant.Model;
using Elephant.Services.TagDataFileManagerService.DTOs;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class HMGRPFile : XXFile, ITDCFile
{
    public const string CommandRegex = @"(?-im)\AFN\sAREA\s.*\sENTITY\s.*\sENT_REF\s.*";
    public HMGRPFile(string filePath) : base(filePath) { }

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
                    var entity = ColumnInfos.First(c => c.Name == "ENTITY");
                    var ent_ref = ColumnInfos.First(c => c.Name == "ENT_REF");

                    var value = line[ent_ref.StartIndex..];

                    if (value.Length == ent_ref.Length)
                    {
                        value = value[..ent_ref.Length];
                    }

                    var tag = new TDCTag()
                    {
                        Name = line.Substring(entity.StartIndex, entity.Length).Trim(),
                        Value = value.Trim(),
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
