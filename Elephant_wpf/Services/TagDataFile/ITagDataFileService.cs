using Elephant.Model;

namespace Elephant.Services.TagDataFile;
public interface ITagDataFileService
{
    public Task GetTagsAsync(string filePath, IProgress<(string, List<Tag>)> p);
    public void WriteTagsToFile(TagsFile newTagsFile, string tagsFilePath);
    public string[] GetTagFilesToImport();
    public TagsFile ReadTagsFile(string tagDataFilePath);
}
