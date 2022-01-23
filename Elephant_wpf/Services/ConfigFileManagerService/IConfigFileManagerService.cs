namespace Elephant.Services.ConfigFileManagerService;

public interface IConfigFileManagerService
{
    public string ConfigFilePath { get; set; }
    public string DataFilePath { get; set; }
    public string ExportFilePath { get; set; }

    public bool UpdateDataFile(string newValue);
    public void UpdateExportFilePath(string newValue);
    public void InitializeFile();
}
