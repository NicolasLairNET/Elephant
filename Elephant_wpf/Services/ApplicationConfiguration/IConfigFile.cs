namespace Elephant.Services.ApplicationConfiguration;

public interface IConfigFile
{
    public string? DataFile { get; set; }
    public string? ExportFile { get; set; }
}
