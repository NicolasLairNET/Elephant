using Elephant.Model;
using Elephant.Services.TagDataFileManagerService.DTOs;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class UCNFile : XXFile, ITDCFile
{

    public UCNFile(string filePath) : base(filePath)
    {
    }

    public List<TDCTag> GetTagsList()
    {
        var tagInfo = new TagInfo()
        {
            NamePosition = new int[2] { 39, 71 },
            Parameter = "ENT_REF",
            ValuePosition = new int[2] { 72, 104 },
            Origin = "UCN",
        };

        //return CreateTagsList(FileContent, tagInfo);
        return new List<TDCTag>();
    }
}