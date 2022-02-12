using Elephant.Model;
using Elephant.Services.ConfigFileManagerService;
using Elephant.Services.JsonFileTDCTag;
using Elephant.Services.JsonFileTDCTag.Helpers;
using System.Windows.Forms;

namespace Elephant.Services;

public sealed class JsonFileTdcTagService : IJsonTdcTagService
{
    public List<TDCTag> TDCTags { get; set; }
    public IConfigFileManagerService configFileManager { get; set; }

    public JsonFileTdcTagService(IConfigFileManagerService configFileManagerService)
    {
        configFileManager = configFileManagerService;
        TDCTags = GetTagsToDataFile(configFileManager.DataFilePath).ToList();
    }

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

            if (tdcFile is null)
            {
                p?.Report((filePath, new List<TDCTag>()));
            }
            else
            {
                p?.Report((filePath, tdcFile.GetTagsList()));
            }
        });
    }

    /// <summary>
    /// Writes a list of tags in DataFile.
    /// </summary>
    /// <param name="newList">list to write in the file</param>
    public void WriteData(List<TDCTag> newList)
    {
        var dataFileUpdated = UpdateDataFile(newList);
        Save(dataFileUpdated);
    }

    private DataFile UpdateDataFile(List<TDCTag> newList)
    {
        using StreamReader reader = new(configFileManager.DataFilePath);
        var dataFile = JsonSerializer.Deserialize<DataFile>(reader.ReadToEnd());
        if (dataFile == null)
        {
            return new DataFile();
        }
        dataFile.Data = newList;
        return dataFile;
    }

    private void Save(DataFile dataFile)
    {
        using StreamWriter writer = new(configFileManager.DataFilePath);
        writer.WriteLine(JsonSerializer.Serialize<DataFile>(dataFile));
    }

    /// <summary>
    /// Open a fileDialog for import TDC Files
    /// </summary>
    /// <returns>List of TDC Files's path</returns>
    public string[] GetPathList()
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
    /// Get all tags from the data file
    /// </summary>
    /// <param name="dataFilePath"></param>
    /// <returns></returns>
    public List<TDCTag> GetTagsToDataFile(string dataFilePath)
    {
        var tags = new List<TDCTag>();
        if (File.Exists(dataFilePath))
        {
            using StreamReader reader = new(dataFilePath);
            var datafile = JsonSerializer.Deserialize<DataFile>(reader.ReadToEnd());

            tags = datafile?.Data;

            if (tags is null)
            {
                return new List<TDCTag>();
            }
        }

        return tags;
    }

    /// <summary>
    /// Search in the list of tags
    /// </summary>
    /// <param name="value">Value to search</param>
    /// <returns>List of tags that matches the search</returns>
    public async Task<List<TDCTag>> Search(string value)
    {
        return await Task.Run(() =>
        {
            Regex regex = new(value.RegexFormat());

            if (value != "")
            {
                return (from tdcTag in TDCTags.AsParallel()
                        let matchName = regex.Matches(tdcTag.Name)
                        let matchValue = regex.Matches(tdcTag.Value)
                        where matchName.Count > 0 || matchValue.Count > 0
                        select tdcTag).ToList();
            }

            return TDCTags;
        }).ConfigureAwait(false);
    }
}
