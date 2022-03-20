using Elephant.Model;
using Elephant.Services.TagDataFileManagerService.DTOs;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;
public class CLAMFile : XXFile, ITDCFile
{
    public const string CommandRegex = @"(?-im)\AFN\s+AM_CP\s.*\sENTITY\s.*\sCL.*\sENT_REF\s.*";
    public CLAMFile(string filePath) : base(filePath){}

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
                var ent_ref = ColumnInfos.First(column => column.Name == "ENT_REF");
                var cl = ColumnInfos.First(column => column.Name == "CL");

                var tag1 = new TDCTag()
                {
                    Name = line.Substring(entity.StartIndex, entity.Length),
                    Value = line.Substring(cl.StartIndex, cl.Length),
                    Parameter = "CL",
                    Origin = "CLAM"
                };

                var tag2 = new TDCTag()
                {
                    Name = line.Substring(entity.StartIndex, entity.Length),
                    Value = line.Substring(ent_ref.StartIndex, ent_ref.Length),
                    Parameter = "ENT_REF",
                    Origin="CLAM"
                };

                tags.Add(tag1);
                tags.Add(tag2);
            }
        }

        return tags;
    }
}