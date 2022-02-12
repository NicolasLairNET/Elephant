using Elephant.Model;
using Elephant.Services.JsonFileTDCTag.DTOs;

namespace Elephant.Services.JsonFileTDCTag.TDCFiles;

public class HMHSTFile : XXFile, ITDCFile
{
    public string[] FileContent { get; set; }

    public HMHSTFile(string filePath)
    {
        FileContent = File.ReadAllLines(filePath);
    }

    public List<TDCTag> GetTagsList()
    {
        var tagDto = new TagInfo()
        {
            NamePosition = new int[2] { 22, 60 },
            Parameter = "ENT_REF",
            ValuePosition = new int[2] { 16, 21 },
            Origin = "HM HST"
        };

        return CreateTagsList(FileContent, tagDto);
    }
}

