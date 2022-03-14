using Elephant.Model;
using Elephant.Services.TagDataFileManagerService.DTOs;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class HMHSTFile : XXFile, ITDCFile
{
    public HMHSTFile(string filePath) : base(filePath)
    {
    }

    public List<TDCTag> GetTagsList()
    {
        var tagInfo = new TagInfo()
        {
            NamePosition = new int[2] { 22, 60 },
            Parameter = "ENT_REF",
            ValuePosition = new int[2] { 16, 21 },
            Origin = "HM HST"
        };

        //return CreateTagsList(FileContent, tagInfo);
        return new List<TDCTag>();
    }
}