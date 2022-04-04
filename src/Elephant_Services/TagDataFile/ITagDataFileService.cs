using Elephant.Model;

namespace Elephant_Services.TagDataFile;
public interface ITagDataFileService
{
    public Task GetTagsAsync(string filePath, IProgress<(string, List<Tag>)> p);
    public void WriteTagsToFile(TagsFile newTagsFile, string tagsFilePath);
    public TagsFile ReadTagsFile(string tagDataFilePath);
}
