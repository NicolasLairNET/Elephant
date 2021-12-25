using Elephant.Model;
using System.Windows.Forms;

namespace Elephant.Services;

public class JsonFileTdcTagService : IJsonTdcTagService
{
    public bool Import(string fileDestination)
    {
        var filePathList = GetPathList();

        if (filePathList.Length == 0)
        {
            return false;
        }

        List<TDCTag> tagList = new();

        foreach (var filePath in filePathList)
        {
            var tdcFile = new TDCFileFactory(filePath).Create();
            if (tdcFile is not null)
            {
                tagList.AddRange(tdcFile.GetTagsList());
            }
            else
            {
                MessageBox.Show($"Le fichier : {Path.GetFileName(filePath)} n'est pas pris en charge par Elephant");
            }
        }

        if (tagList.Count == 0)
        {
            return false;
        }

        // delete similar tags
        tagList = tagList.Distinct(new TDCTagComparer()).ToList();

        var tagListSerialized = JsonSerializer.Serialize(tagList);
        using StreamWriter writer = new StreamWriter(fileDestination);
        writer.WriteAsync(tagListSerialized);

        return true;
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

