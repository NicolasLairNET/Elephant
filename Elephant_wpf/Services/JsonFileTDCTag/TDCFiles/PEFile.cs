using Elephant.Model;
using Elephant.Services.JsonFileTDCTag.DTOs;
using Elephant.Services.JsonFileTDCTag.TDCFiles;

namespace Elephant.Services;

public class UCNFile : XXFile, ITDCFile
{
    public string[] FileContent { get; set; }

    public UCNFile(string filePath)
    {
        FileContent = File.ReadAllLines(filePath);
    }

    public List<TDCTag> GetTagsList()
    {
        var tagDto = new TagDto()
        {
            NamePosition = new int[2] { 39, 71 },
            Parameter = "ENT_REF",
            ValuePosition = new int[2] { 72, 104 },
            Origin = "UCN",
        };

        return CreateTagsList(FileContent, tagDto);
    }
}

