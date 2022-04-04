namespace Elephant.Services.ApplicationConfiguration;

public interface IConfigFileService
{
    public string ConfigFilePath { get; set; }
    public string DataFilePath { get; set; }
    public string ExportFilePath { get; set; }

    public bool UpdateDataFile(string newValue);
    public void UpdateExportFilePath(string newValue);
    public void InitializeFile();
}
