using Elephant.Model;

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
        ExportFilePath = Path.Combine(Directory.GetCurrentDirectory(), "export.csv");
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

    public void UpdateDataFile(string newValue)
    {
        DataFilePath = newValue;
        var newConfig = new ConfigFile { DataFile = DataFilePath, ExportFile = ExportFilePath };
        if (!File.Exists(newValue))
        {
            CreateDataFile(newValue);
        }
        EditConfigFile(newConfig);
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
        if (File.Exists(newValue))
        {
            ExportFilePath = newValue;
            var newConfig = new ConfigFile { DataFile = DataFilePath, ExportFile = ExportFilePath };
            using StreamWriter writer = new(ConfigFilePath);
            writer.WriteLine(JsonSerializer.Serialize(newConfig));
        }
    }
}
