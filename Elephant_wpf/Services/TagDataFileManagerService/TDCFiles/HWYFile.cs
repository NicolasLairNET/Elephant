using Elephant.Model;
using Elephant.Services.TagDataFileManagerService.DTOs;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class HWYFile : XXFile, ITDCFile
{
    public HWYFile(string filePath) : base(filePath)
    {
    }

    public List<TDCTag> GetTagsList()
    {
        var tagInfo = new TagInfo()
        {
            NamePosition = new int[2] { 16, 51 },
            Parameter = "ENT_REF",
            ValuePosition = new int[2] { 52, 90 },
            Origin = "HIWAY"
        };

        //return CreateTagsList(FileContent, tagInfo);
        return new List<TDCTag>();
    }
}