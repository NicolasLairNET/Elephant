namespace Elephant.Services.ConfigFileManagerService;

public interface IConfigFile
{
    public string? DataFile { get; set; }
    public string? ExportFile { get; set; }
}
