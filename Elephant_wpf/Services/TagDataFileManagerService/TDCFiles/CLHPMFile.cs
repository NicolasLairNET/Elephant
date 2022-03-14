using Elephant.Model;
using Elephant.Services.TagDataFileManagerService.DTOs;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public class CLHPMFile : XXFile, ITDCFile
{

    public CLHPMFile(string filePath) : base(filePath)
    {
        //FileContent = File.ReadAllLines(filePath);
    }

    public List<TDCTag> GetTagsList()
    {
        var tagInfo = new TagInfo()
        {
            NamePosition = new int[2] { 20, 36 },
            Parameter = "ENT_REF",
            ValuePosition = new int[2] { 46, 84 },
            Origin = "CLHPM"
        };

        //return CreateTagsList(FileContent, tagInfo);
        return new List<TDCTag>();
    }
}

