using Elephant.Model;
using Elephant.Services.JsonFileTDCTag.DTOs;
using Elephant.Services.JsonFileTDCTag.TDCFiles;

namespace Elephant.Services;

public class PEFile : XXFile, ITDCFile
{
    public string[] FileContent { get; set; }

    public PEFile(string filePath)
    {
        FileContent = File.ReadAllLines(filePath);
    }

    public List<TDCTag> GetTagsList()
    {
        var tagInfo = new TagInfo()
        {
            NamePosition = new int[2] { 19, 57 },
            Parameter = "ENT_REF",
            ValuePosition = new int[2] { 10, 18 },
            Origin = "PE"
        };

        return CreateTagsList(FileContent, tagInfo);
    }
}

