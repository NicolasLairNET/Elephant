using Elephant.Model;
using Elephant.Services.TagDataFileManagerService.DTOs;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public abstract class XXFile
{
    /// <summary>
    /// Create a TDCTag list with the content of the file.
    /// </summary>
    /// <param name="fileContent">Content of the file</param>
    /// <param name="tagInfo"></param>
    /// <returns>The list of TDCTags in the file </returns>
    public List<TDCTag> CreateTagsList(string[] fileContent, TagInfo tagInfo)
    {
        List<TDCTag> tagList = new();
        TDCTag tag = new();
        foreach (string line in fileContent)
        {
            if (line.Length > 3 && line[0..3] == "NET")
            {
                tag = new()
                {
                    Name = line[tagInfo.NamePosition[0]..tagInfo.NamePosition[1]].Trim(),
                    Parameter = tagInfo.Parameter,
                    Value = line[tagInfo.ValuePosition[0]..tagInfo.ValuePosition[1]].Trim(),
                    Origin = tagInfo.Origin
                };
                tagList.Add(tag);
            }
        }

        return tagList;
    }
}