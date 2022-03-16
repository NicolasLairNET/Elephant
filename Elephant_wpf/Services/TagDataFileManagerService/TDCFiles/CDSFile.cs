using Elephant.Model;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class CDSFile : XXFile, ITDCFile
{
    public CDSFile(string filePath) : base (filePath) {}

    public const string CommandRegex = @"(?-im)\AFN\s+AM_CP\s.*\sENTITY\s(?:(?!\bCL\b).)*\sENT_REF\s.*";
 

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
                   Origin = "CDS"
                };
                tags.Add(tag);
            }
        }

        return tags;
    }
}
