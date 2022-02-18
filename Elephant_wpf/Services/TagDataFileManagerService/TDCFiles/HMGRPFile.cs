using Elephant.Model;
using Elephant.Services.TagDataFileManagerService.DTOs;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class HMGRPFile : XXFile, ITDCFile
{
    public string[] FileContent { get; set; }

    public HMGRPFile(string filePath)
    {
        FileContent = File.ReadAllLines(filePath);
    }

    public List<TDCTag> GetTagsList()
    {
        var tagInfo = new TagInfo()
        {
            NamePosition = new int[2] { 52, 90 },
            Parameter = "ENT_REF",
            ValuePosition = new int[2] { 16, 51 },
            Origin = "HM GRP"
        };

        return CreateTagsList(FileContent, tagInfo);
    }
}

