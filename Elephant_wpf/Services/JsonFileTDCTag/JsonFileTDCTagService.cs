using System.Windows.Forms;
using Elephant.Model;
using Elephant.Services.JsonFileTDCTag.Helpers;

namespace Elephant.Services;

internal class JsonFileTdcTagService
{
    private static string JsonFileName => Path.Combine(Directory.GetCurrentDirectory(), "DATA.json");

    public JsonFileTdcTagService()
    {
        if (File.Exists(JsonFileName)) return;
        StreamWriter sw = new(JsonFileName);
        sw.WriteLine("[]");
        sw.Close();
    }

    public ObservableCollection<TDCTag> GetTDCTags()
    {
        try
        {
            using var jsonFileReader = File.OpenText(JsonFileName);
            return JsonSerializer.Deserialize<ObservableCollection<TDCTag>>(
                jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
        catch (FileNotFoundException)
        {
            MessageBox.Show("Le fichier de données à été supprimé, relancez l'application.");
            return null;
        }
    }

    public ObservableCollection<TDCTag> Import()
    {
        var filePathList = GetPathList();

        MessageBox.Show(CreateJsonFile(filePathList) ? "Import terminé" : "Aucun fichier importé");

        return GetTDCTags();
    }

    public ObservableCollection<TDCTag> Search(string value)
    {
        var data = GetTDCTags();
        Regex regex = new(value.RegexFormat());

        return value != ""
            ? new ObservableCollection<TDCTag>(
            from tdcTag in data
            let matchName = regex.Matches(tdcTag.Name)
            let matchValue = regex.Matches(tdcTag.Value)
            where matchName.Count > 0 || matchValue.Count > 0
            select tdcTag)
            : data;
    }

    private bool CreateJsonFile(string[] filePathList)
    {
        if (!filePathList.Any())
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

        if (!tagList.Any())
        {
            return false;
        }

        // delete similar tags
        tagList = tagList.Distinct(new TDCTagComparer()).ToList();

        var tagListSerialized = JsonSerializer.Serialize(tagList);
        File.WriteAllText(JsonFileName, tagListSerialized);

        return true;
    }

    /// <summary>
    /// Open a fileDialog for import TDC Files
    /// </summary>
    /// <returns>List of TDC Files's path</returns>
    private string[] GetPathList()
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

