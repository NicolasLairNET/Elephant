using Elephant.Model;

namespace Elephant_Services.TagDataFile.FileType;

public class InvalidFile : ITDCFile
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string[] FileContent { get; set; }
    public List<Tag> Tags { get; set; } = new List<Tag>();

    public InvalidFile(string filePath)
    {
        FileName = Path.GetFileName(filePath);
        FilePath = filePath;
        FileContent = Array.Empty<string>();
    }

    public void GetTagsList()
    {
        return;
    }
}
