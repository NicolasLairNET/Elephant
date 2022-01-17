namespace Elephant.Services.ConfigFileManagerService;

public interface IConfigFileManagerService
{
    public string ConfigFilePath { get; set; }
    public string DataFilePath { get; set; }
    public string ExportFilePath { get; set; }

    public void UpdateDataFile(string newValue);
    public void UpdateExportFile(string newValue);
    public void InitializeFile();
}
