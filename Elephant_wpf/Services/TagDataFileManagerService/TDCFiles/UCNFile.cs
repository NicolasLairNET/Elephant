using Elephant.Model;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class UCNFile : XXFile, ITDCFile
{
    public const string CommandRegex = @"(?-im)\AFN\sUCN_CP\s.*\sNODE_CP\s.*\sMODULE\s.*\sSLOT\s.\sENTITY\s.*\sENT_REF\s.*\sRN\s.*\sREF_MOD\s.*\sREF_SL\s.*";
    public UCNFile(string filePath) : base(filePath) { }

    public List<TDCTag> GetTagsList()
    {
        List<TDCTag> tags = new();

        if (ColumnInfos == null)
        {
            return tags;
        }

        for (int i = 0; i < FileContent.Length; i++)
        {
            if (FileContent[i].Contains("NET"))
            {
                var entity = ColumnInfos.First(column => column.Name == "ENTITY");
                var value = ColumnInfos.First(column => column.Name == "ENT_REF");

                var tag = new TDCTag()
                {
                    Name = FileContent[i].Substring(entity.StartIndex, entity.Length).Trim(),
                    Value = FileContent[i].Substring(value.StartIndex, value.Length).Trim(),
                    Parameter = "ENT_REF",
                    Origin = "UCN"
                };
                tags.Add(tag);
            }
        }

        return tags;
    }
}