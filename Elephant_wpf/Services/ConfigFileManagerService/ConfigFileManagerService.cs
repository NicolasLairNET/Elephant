using Elephant.Model;
using System.Windows.Forms;

namespace Elephant.Services.ConfigFileManagerService;

public class ConfigFileManager : IConfigFileManagerService
{
    public string ConfigFilePath { get; set; }
    public string DataFilePath { get; set; }
    public string ExportFilePath { get; set; }

    public ConfigFileManager()
    {
        ConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
        DataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data.json");
        ExportFilePath = Directory.GetCurrentDirectory();
        InitializeFile();
    }

    public void InitializeFile()
    {

        if (File.Exists(ConfigFilePath))
        {
            using StreamReader reader = new(ConfigFilePath);
            var configFile = JsonSerializer.Deserialize<ConfigFile>(reader.ReadToEnd());
            if (configFile?.DataFile != null && configFile?.ExportFile != null)
            {
                DataFilePath = configFile.DataFile;
                ExportFilePath = configFile.ExportFile;
            }
        }
        else
        {
            var newConfigFile = new ConfigFile { DataFile = DataFilePath, ExportFile = ExportFilePath };
            EditConfigFile(newConfigFile);
        }
    }

    public bool UpdateDataFile(string newValue)
    {
        if (!File.Exists(newValue))
        {
            DialogResult choice = MessageBox.Show($"Le fichier {newValue} n'existe pas voulez-vous le créer ?", "Création fichier", MessageBoxButtons.YesNo);
            if (choice != DialogResult.Yes)
            {
                return false;
            }
            CreateDataFile(newValue);
        }
        DataFilePath = newValue;
        var newConfig = new ConfigFile { DataFile = DataFilePath, ExportFile = ExportFilePath };
        EditConfigFile(newConfig);

        return true;
    }

    private void EditConfigFile(ConfigFile configFile)
    {
        using StreamWriter writer = new(ConfigFilePath);
        writer.WriteLine(JsonSerializer.Serialize(configFile));
    }

    private void CreateDataFile(string path)
    {
        File.Create(path).Dispose();
        using StreamWriter writer = new(path);
        writer.WriteLine("[]");
    }

    public void UpdateExportFile(string newValue)
    {
        if (Directory.Exists(newValue))
        {
            ExportFilePath = newValue;
            var newConfig = new ConfigFile { DataFile = DataFilePath, ExportFile = ExportFilePath };
            using StreamWriter writer = new(ConfigFilePath);
            writer.WriteLine(JsonSerializer.Serialize(newConfig));
        }
    }
}
