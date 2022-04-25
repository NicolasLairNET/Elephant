using Elephant.Model;
using Elephant_Services.TagDataFile.FileType;
using System.Text.Json;

namespace Elephant_Services.TagDataFile;

public class TagDataFileService : ITagDataFileService
{
    /// <summary>
    /// Get the list of tags from a tdc file.
    /// </summary>
    /// <param name="filePath">path to tdc file</param>
    /// <param name="p">Task progress management, allows to know when the task is finished</param>
    /// <returns>list of tags or an empty list if the file isn't a tdc file</returns>
    public async Task GetTagsAsync(string filePath, IProgress<ITDCFile> p)
    {
        await Task.Run(() =>
        {
            var tdcFile = new TagFileFactory().Create(filePath);
            if (tdcFile != null)
            {
                p?.Report(tdcFile);
            }
        });
    }

    /// <summary>
    /// Serialize the TagDataFile object and write in the DataFile
    /// </summary>
    /// <param name="newTagDataFile">New TagDataFile object to serialize.</param>
    /// <param name="dataFilePath">Path of the data file to be written.</param>
    public void WriteTagsToFile(TagsFile newTagsFile, string tagsFilePath)
    {
        using StreamWriter writer = new StreamWriter(tagsFilePath);
        var tagDataFileSerialized = JsonSerializer.Serialize<TagsFile>(newTagsFile);
        writer.Write(tagDataFileSerialized);
    }

    /// <summary>
    /// Deserialize a TagDataFile return an empty TagDataFile if the deserialize is impossible.
    /// </summary>
    /// <param name="tagDataFilePath">Path of the file to be deserialized.</param>
    public TagsFile ReadTagsFile(string tagDataFilePath)
    {
        if (!File.Exists(tagDataFilePath))
        {
            return new TagsFile();
        }

        using StreamReader reader = new(tagDataFilePath);
        var datafile = JsonSerializer.Deserialize<TagsFile>(reader.ReadToEnd());

        return datafile ?? new TagsFile();
    }
}
