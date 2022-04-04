namespace Elephant_Services.ApplicationConfiguration;

public class ConfigFile : IConfigFile
{
    public string? DataFile { get; set; }
    public string? ExportFile { get; set; }
}
