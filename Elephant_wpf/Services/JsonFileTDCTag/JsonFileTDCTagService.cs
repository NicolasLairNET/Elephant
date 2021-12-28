using Elephant.Model;
using System.Windows.Forms;

namespace Elephant.Services;

public class JsonFileTdcTagService : IJsonTdcTagService
{
    /// <summary>
    /// Import the selected files into the json file
    /// </summary>
    /// <param name="fileDestination">Destination JSON File</param>
    public void Import(string fileDestination)
    {
        var filePathList = GetPathList();

        if (filePathList.Length == 0) return;

        List<TDCTag> tagList = new();

        foreach (string filePath in filePathList)
        {
            var list = ConvertFileToTagList(filePath);
            if (list.Any())
            {
                tagList.AddRange(list);
            }
        }

        if (tagList.Count == 0) return;

        // delete similar tags
        tagList = tagList.Distinct(new TDCTagComparer()).ToList();
        using StreamWriter writer = new(fileDestination);
        writer.Write(SerializeTagsList(tagList));
    }

    public static IEnumerable<TDCTag> ConvertFileToTagList(string file)
    {
        List<TDCTag> list = new();

        var tdcFile = new TDCFileFactory(file).Create();

        list = tdcFile.GetTagsList();

        return list;
    }

    public static string SerializeTagsList(IEnumerable<TDCTag> list)
    {
        return JsonSerializer.Serialize(list);
    }

    /// <summary>
    /// Open a fileDialog for import TDC Files
    /// </summary>
    /// <returns>List of TDC Files's path</returns>
    private static string[] GetPathList()
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
}

