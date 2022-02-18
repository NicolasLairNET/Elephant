using Elephant.Model;

namespace Elephant.Services.TagDataFileManagerService;
public interface ITagDataFileManager
{
    public Task GetTagsAsync(string filePath, IProgress<(string, List<TDCTag>)> p);
    public void WriteTagDataToFile(TagDataFile newTagDataFile, string dataFilePath);
    public string[] GetTagFilesToImport();
    public TagDataFile ReadTagDataFile(string tagDataFilePath);
}
