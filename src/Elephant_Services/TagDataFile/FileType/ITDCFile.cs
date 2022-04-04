using Elephant.Model;

namespace Elephant_Services.TagDataFile.FileType;

public interface ITDCFile
{
    public List<Tag>? GetTagsList();
}