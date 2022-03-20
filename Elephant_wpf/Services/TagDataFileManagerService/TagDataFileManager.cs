using Elephant.Model;
using System.Windows.Forms;

namespace Elephant.Services.TagDataFileManagerService;

public class TagDataFileManager : ITagDataFileManager
{
    /// <summary>
    /// Get the list of tags from a tdc file.
    /// </summary>
    /// <param name="filePath">path to tdc file</param>
    /// <param name="p">Task progress management, allows to know when the task is finished</param>
    /// <returns>list of tags or an empty list if the file isn't a tdc file</returns>
    public async Task GetTagsAsync(string filePath, IProgress<(string, List<TDCTag>)> p)
    {
        await Task.Run(() =>
        {
            var tdcFile = new TDCFileFactory(filePath).Create();
            var TagsList = tdcFile?.GetTagsList();

            if (TagsList != null)
            {
                p?.Report((filePath, TagsList));
            }
        });
    }

    /// <summary>
    /// Serialize the TagDataFile object and write in the DataFile
    /// </summary>
    /// <param name="newTagDataFile">New TagDataFile object to serialize.</param>
    /// <param name="dataFilePath">Path of the data file to be written.</param>
    public void WriteTagDataToFile(TagDataFile newTagDataFile, string dataFilePath)
    {
        using StreamWriter writer = new StreamWriter(dataFilePath);
        var tagDataFileSerialized = JsonSerializer.Serialize<TagDataFile>(newTagDataFile);
        writer.Write(tagDataFileSerialized);
    }

    /// <summary>
    /// Open a fileDialog for import TDC Files
    /// </summary>
    /// <returns>List of TDC Files's path</returns>
    public string[] GetTagFilesToImport()
    {
        var pathList = Array.Empty<string>();
        OpenFileDialog openFileDialog = new()
        {
            InitialDirectory = "c:\\",
            RestoreDirectory = true,
            Multiselect = true
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            pathList = openFileDialog.FileNames;
        }

        return pathList;
    }

    /// <summary>
    /// Deserialize a TagDataFile return an empty TagDataFile if the deserialize is impossible.
    /// </summary>
    /// <param name="tagDataFilePath">Path of the file to be deserialized.</param>
    public TagDataFile ReadTagDataFile(string tagDataFilePath)
    {
        if (!File.Exists(tagDataFilePath))
        {
            return new TagDataFile();
        }

        using StreamReader reader = new(tagDataFilePath);
        var datafile = JsonSerializer.Deserialize<TagDataFile>(reader.ReadToEnd());

        return datafile ?? new TagDataFile();
    }
}
