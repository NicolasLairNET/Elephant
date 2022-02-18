using Elephant.Model;
using Elephant.Services.TagDataFileManagerService.DTOs;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;
public class CDSFile : XXFile, ITDCFile
{
    public string[] FileContent { get; set; }

    public CDSFile(string filePath)
    {
        FileContent = File.ReadAllLines(filePath);
    }

    public List<TDCTag> GetTagsList()
    {
        var tagDto = new TagInfo()
        {
            NamePosition = new int[2] { 16, 51 },
            Parameter = "ENT_REF",
            ValuePosition = new int[2] { 52, 90 },
            Origin = "CDS"
        };

        return CreateTagsList(FileContent, tagDto);
    }
}