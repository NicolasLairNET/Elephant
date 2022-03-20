using Elephant.Model;
using Elephant.Services.TagDataFileManagerService.DTOs;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class CLHPMFile : XXFile, ITDCFile
{
    public const string CommandRegex = @"(?-im)\AFN\s+UCN_CP\s.*\sPM\s.\sPMOD_ENT\s.*PM_SEQ\s.*\sENT_REF\s.*\sRN\s.*\sREF_MOD\s.*\sREF_SL\s.*";

    public CLHPMFile(string filePath) : base(filePath) { }

    public List<TDCTag>? GetTagsList()
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
                var pmod_ent = ColumnInfos.First(c => c.Name == "PMOD_ENT");
                var pm_seq = ColumnInfos.First(c => c.Name == "PM_SEQ");
                var ent_ref = ColumnInfos.First(c => c.Name == "ENT_REF");

                var tag1 = new TDCTag()
                {
                    Name = line.Substring(pmod_ent.StartIndex, pmod_ent.Length),
                    Value = line.Substring(pm_seq.StartIndex, pm_seq.Length),
                    Parameter = "PM_SEQ",
                    Origin = "CLHPM"
                };

                var tag2 = new TDCTag()
                {
                    Name = line.Substring(pmod_ent.StartIndex, pmod_ent.Length),
                    Value = line.Substring(ent_ref.StartIndex, ent_ref.Length),
                    Parameter = "ENT_REF",
                    Origin = "CLHPM"
                };

                tags.Add(tag1);
                tags.Add(tag2);
            }
        }

        return tags;
    }
}