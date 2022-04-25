using Elephant.Model;
using Elephant_Services.TagDataFile.FileType;

namespace Elephant_Services.TagDataFile;
public interface ITagDataFileService
{
    public Task GetTagsAsync(string filePath, IProgress<ITDCFile> p);
    public void WriteTagsToFile(TagsFile newTagsFile, string tagsFilePath);
    public TagsFile ReadTagsFile(string tagDataFilePath);
}
