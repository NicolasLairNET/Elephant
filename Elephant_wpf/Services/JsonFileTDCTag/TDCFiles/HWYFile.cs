using Elephant.Model;
using Elephant.Services.JsonFileTDCTag.DTOs;

namespace Elephant.Services.JsonFileTDCTag.TDCFiles;

public class HWYFile : XXFile, ITDCFile
{
    public string[] FileContent { get; set; }

    public HWYFile(string filePath)
    {
        FileContent = File.ReadAllLines(filePath);
    }

    public List<TDCTag> GetTagsList()
    {
        var tagDto = new TagDto()
        {
            NamePosition = new int[2] { 16, 51 },
            Parameter = "ENT_REF",
            ValuePosition = new int[2] { 52, 90 },
            Origin = "HIWAY"
        };

        return CreateTagsList(FileContent, tagDto);
    }
}

