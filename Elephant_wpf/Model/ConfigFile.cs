using Elephant.Services.ConfigFileManagerService;

namespace Elephant.Model;

public class ConfigFile : IConfigFile
{
    public string? DataFile { get; set; }
    public string? ExportFile { get; set; }
}
