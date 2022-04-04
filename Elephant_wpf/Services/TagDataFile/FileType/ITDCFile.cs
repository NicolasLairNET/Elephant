using Elephant.Model;

namespace Elephant.Services.TagDataFile.FileType;

public interface ITDCFile
{
    public List<Tag>? GetTagsList();
}