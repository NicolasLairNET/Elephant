using Elephant.DTOs;
using Elephant.Model;

namespace Elephant.Services;

public class CLHPMFile : XXFile, ITDCFile
{
    public string[] FileContent { get; set; }

    public CLHPMFile(string filePath)
    {
        FileContent = File.ReadAllLines(filePath);
    }

    public List<TDCTag> GetTagsList()
    {
        var tagDto = new TagDto()
        {
            NamePosition = new int[2] { 20, 36 },
            Parameter = "ENT_REF",
            ValuePosition = new int[2] { 46, 84 },
            Origin = "CLHPM"
        };

        return CreateTagsList(FileContent, tagDto);
    }
}

