using Elephant.DTOs;
using Elephant.Model;

namespace Elephant.Services;
public class CDSFile : XXFile, ITDCFile
{
    public string[] FileContent { get; set; }

    public CDSFile(string filePath)
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
            Origin = "CDS"
        };

        return CreateTagsList(FileContent, tagDto);
    }
}

