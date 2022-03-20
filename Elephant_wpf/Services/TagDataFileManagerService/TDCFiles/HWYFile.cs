using Elephant.Model;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class HWYFile : XXFile, ITDCFile
{
    public const string CommandRegex = @"(?-im)\AFN\s+HWY_CP\s.*\sENTITY\s.*\sENT_REF\s.*";
    public HWYFile(string filePath) : base(filePath) { }

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
                var entity = ColumnInfos.First(column => column.Name == "ENTITY");
                var value = ColumnInfos.First(column => column.Name == "ENT_REF");

                var tag = new TDCTag()
                {
                    Name = line.Substring(entity.StartIndex, entity.Length).Trim(),
                    Value = line.Substring(value.StartIndex, value.Length).Trim(),
                    Parameter = "ENT_REF",
                    Origin = "HIWAY"
                };
                tags.Add(tag);
            }
        }

        return tags;
    }
}