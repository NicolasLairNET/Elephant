using Elephant.Model;
using System.Windows.Forms;

namespace Elephant.Services.ApplicationConfiguration;

public class ConfigFileService : IConfigFileService
{
    public string ConfigFilePath { get; set; }
    public string DataFilePath { get; set; }
    public string ExportFilePath { get; set; }

    public ConfigFileService()
    {
        ConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
        DataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data.json");
        ExportFilePath = Directory.GetCurrentDirectory();
        InitializeFile();
    }

    /// <summary>
    /// Creates the config file if not exist with default values
    /// </summary>
    public void InitializeFile()
    {
        if (File.Exists(ConfigFilePath))
        {
            using StreamReader reader = new(ConfigFilePath);
            var configFile = JsonSerializer.Deserialize<ConfigFile>(reader.ReadToEnd());
            if (configFile?.DataFile == null || configFile?.ExportFile == null) return;
            DataFilePath = configFile.DataFile;
            ExportFilePath = configFile.ExportFile;
        }
        else
        {
            var newConfigFile = new ConfigFile { DataFile = DataFilePath, ExportFile = ExportFilePath };
            EditConfigFile(newConfigFile);
        }
    }

    /// <summary>
    /// Update the file which store the datas
    /// And edits the config file with the new path for data file
    /// </summary>
    /// <param name="newValue"></param>
    /// <returns></returns>
    public bool UpdateDataFile(string newValue)
    {
        if (!File.Exists(newValue))
        {
            DialogResult choice = MessageBox.Show($"Le fichier {newValue} n'existe pas voulez-vous le créer ?", "Création fichier", MessageBoxButtons.YesNo);
            if (choice != DialogResult.Yes)
            {
                return false;
            }
            if (!CreateDataFile(newValue))
            {
                return false;
            }
        }

        DataFilePath = newValue;
        var newConfig = new ConfigFile { DataFile = DataFilePath, ExportFile = ExportFilePath };

        return EditConfigFile(newConfig);
    }

    /// <summary>
    /// Writes in the config file the new configuration for the app
    /// </summary>
    /// <param name="configFile">Path to config file</param>
    /// <returns>true if the file is edited otherwise false</returns>
    private bool EditConfigFile(ConfigFile configFile)
    {
        try
        {
            using StreamWriter writer = new(ConfigFilePath);
            writer.WriteLine(JsonSerializer.Serialize(configFile));
        }
        catch (Exception)
        {
            MessageBox.Show(
                "Le fichier de config n'a pas été mis à jour.\nAnnulation de la modification.",
                "Erreur: Modification du fichier de config.",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Create the json file which store datas
    /// </summary>
    /// <param name="path">Path of file</param>
    /// <returns>true if the file is created otherwise false</returns>
    private static bool CreateDataFile(string path)
    {
        try
        {
            File.Create(path).Dispose();
            using StreamWriter writer = new(path);
            var newDataFile = new TagsFile();
            JsonSerializer.Serialize(newDataFile);
            writer.WriteLine(JsonSerializer.Serialize(newDataFile));
        }
        catch (DirectoryNotFoundException)
        {
            MessageBox.Show(
                "Le chemin sélectionné n'existe pas.\nAnnulation de la modification.",
                "Erreur: Création du fichier de données.",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Updated the default path for store the export files
    /// </summary>
    /// <param name="newValue"></param>
    public void UpdateExportFilePath(string newValue)
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
