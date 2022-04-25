using Elephant.Model;

namespace Elephant_Services.TagDataFile.FileType;

public interface ITDCFile
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string[] FileContent { get; set; }
    public List<Tag> Tags { get; set; }
    public void GetTagsList();
}