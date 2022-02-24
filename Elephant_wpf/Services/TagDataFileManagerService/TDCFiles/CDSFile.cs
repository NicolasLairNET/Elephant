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

    public static string commandRegex = @"\b[FN]*[AM_CP]*[ENTITY]*[ENT_REF]";

    public List<TDCTag> GetTagsList()
    {
        List<string> header = new();
        List<TDCTag> tags = new();
        foreach (string line in FileContent)
        {
            if (line.Contains("MEDIA"))
            {
                header = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                continue;
            }

            if (line.Contains("NET"))
            {
                var lineValue = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var tag = new TDCTag()
                {
                    Name = lineValue[header.IndexOf("ENTITY")],
                    Value = lineValue[header.IndexOf("ENT_REF")],
                    Parameter = "ENT_REF",
                    Origin = "CDS"
                };

                tags.Add(tag);
            }
        }

        return tags;
    }
}