using Elephant.Model;
using Elephant.Services.TagDataFileManagerService.DTOs;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;
public class CLAMFile : XXFile, ITDCFile
{
    public string[] FileContent { get; set; }

    public CLAMFile(string filePath)
    {
        FileContent = File.ReadAllLines(filePath);
    }

    public List<TDCTag> GetTagsList()
    {
        var tagInfo = new TagInfo()
        {
            NamePosition = new int[2] { 16, 51 },
            Parameter = "CL",
            ValuePosition = new int[2] { 52, 60 },
            Origin = "CL AM"
        };

        return CreateTagsList(FileContent, tagInfo);
    }
}

